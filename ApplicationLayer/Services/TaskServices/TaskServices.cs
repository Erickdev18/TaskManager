using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repository.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Collections.Concurrent;

namespace ApplicationLayer.Services.TaskServices
{
    public class TaskServices
    {
        private readonly ICommonProcess<Tareas> _commonProcess;

        // Cola reactiva para procesar tareas secuencialmente
        private readonly Subject<Func<Task>> _taskQueue = new();
        private readonly ConcurrentQueue<Func<Task>> _pendingTasks = new();
        private bool _isProcessing = false;

        // Delegado para validar tareas antes de guardarlas
        public Func<Tareas, bool> ValidateTask { get; set; } = tarea =>
            !string.IsNullOrWhiteSpace(tarea.Description) && tarea.DueDate > DateTime.Now;

        // Action para notificar cuando se crea o elimina una tarea
        public Action<string> Notify { get; set; } = message => { Console.WriteLine($"Notificación: {message}"); };

        // Func para calcular días restantes para completar una tarea
        public Func<Tareas, int> DaysRemaining { get; set; } = tarea =>
            (tarea.DueDate - DateTime.Now).Days;
        public TaskServices(ICommonProcess<Tareas> commonProcess)
        {
            _commonProcess = commonProcess;

            // Suscribirse al Subject para procesar tareas secuencialmente
            _taskQueue.Subscribe(async taskFunc =>
            {
                _pendingTasks.Enqueue(taskFunc);
                await ProcessQueueAsync();
            });
        }

        // Método para agregar una tarea a la cola
        public void EnqueueTask(Func<Task> taskFunc)
        {
            _taskQueue.OnNext(taskFunc);
        }

        private readonly List<string> _failedTaskLogs = new(); // Lista para registrar errores

        // Procesa la cola de tareas secuencialmente con manejo de errores
        private async Task ProcessQueueAsync()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            while (_pendingTasks.TryDequeue(out var nextTask))
            {
                int maxRetries = 3;
                int attempt = 0;
                bool success = false;

                while (attempt < maxRetries && !success)
                {
                    try
                    {
                        await nextTask();
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        attempt++;
                        string errorMsg = $"Error procesando tarea (intento {attempt}): {ex.Message}";
                        Notify?.Invoke(errorMsg);
                        _failedTaskLogs.Add(errorMsg); // Registrar el error
                        if (attempt < maxRetries)
                            await Task.Delay(1000); // Espera 1 segundo antes de reintentar
                    }
                }

                if (!success)
                {
                    string finalMsg = "La tarea ha fallado después de varios intentos.";
                    Notify?.Invoke(finalMsg);
                    _failedTaskLogs.Add(finalMsg); // Registrar el fallo definitivo
                }
            }

            _isProcessing = false;
        }

        // Método para obtener los errores registrados
        public IReadOnlyList<string> GetFailedTaskLogs()
        {
            return _failedTaskLogs.AsReadOnly();
        }

        public async Task<Response<Tareas>> GetTaskAllAsync()
        {
            var response = new Response<Tareas>();
            try
            {
                response.DataList = await _commonProcess.GetAllAsync();
                response.Succesful = true;
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }
        public async Task<Response<Tareas>> GetTaskByIdAllAsync(int id)
        {
            var response = new Response<Tareas>();
            try
            {
                var result = await _commonProcess.GetByIdAsync(id);
                if (result != null)
                {
                    response.SingleData = result;
                    response.Succesful = true;
                }
                else
                {
                    response.Succesful = false;
                    response.Message = "No se encontro informacion";
                }
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }
        public async Task<Response<string>> AddTaskAllAsync(Tareas tarea)
        {
            var response = new Response<string>();
            try
            {
                // Validación con delegado
                if (!ValidateTask(tarea))
                {
                    response.Succesful = false;
                    response.Message = "La tarea no es válida.";
                    return response;
                }

                // Encolar la tarea para procesamiento secuencial
                EnqueueTask(async () =>
                {
                    var result = await _commonProcess.AddAsync(tarea);
                    response.Message = result.Message;
                    response.Succesful = result.IsSuccess;
                    if (result.IsSuccess)
                        Notify?.Invoke($"Tarea creada: {tarea.Description}");
                    if (result.IsSuccess)
                        response.Message += $" Días restantes: {DaysRemaining(tarea)}";
                });
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }
        // Encolar actualización de tarea
        public Task<Response<string>> UpdateTaskAllAsync(Tareas tarea)
        {
            var response = new Response<string>();
            EnqueueTask(async () =>
            {
                try
                {
                    var result = await _commonProcess.UpdateAsync(tarea);
                    response.Message = result.Message;
                    response.Succesful = result.IsSuccess;
                    if (result.IsSuccess)
                        Notify?.Invoke($"Tarea actualizada: {tarea.Description}");
                }
                catch (Exception e)
                {
                    response.Errors.Add(e.Message);
                    Notify?.Invoke($"Error al actualizar tarea: {e.Message}");
                }
            });
            return Task.FromResult(response);
        }

        // Encolar eliminación de tarea
        public Task<Response<string>> DeleteTaskAllAsync(int id)
        {
            var response = new Response<string>();
            EnqueueTask(async () =>
            {
                try
                {
                    var result = await _commonProcess.DeleteAsync(id);
                    response.Message = result.Message;
                    response.Succesful = result.IsSuccess;
                    if (result.IsSuccess)
                        Notify?.Invoke($"Tarea eliminada: {id}");
                }
                catch (Exception e)
                {
                    response.Errors.Add(e.Message);
                    Notify?.Invoke($"Error al eliminar tarea: {e.Message}");
                }
            });
            return Task.FromResult(response);
        }

        // Agrega una instancia de TaskCache
        private readonly TaskCache _taskCache = new();

        // Ejemplo: método para calcular el porcentaje de tareas completadas usando caché
        public async Task<double> CalculateTaskCompletionRateAsync()
        {
            var cachedRate = _taskCache.GetCompletionRate();
            if (cachedRate.HasValue)
                return cachedRate.Value;

            var tareas = await _commonProcess.GetAllAsync();
            int total = tareas.Count();
            int completadas = tareas.Count(t => t.Status == "Completed");
            double rate = total == 0 ? 0 : (double)completadas / total * 100;
            _taskCache.SetCompletionRate(rate);
            return rate;
        }

        // método para filtrar tareas usando caché
        public async Task<List<Tareas>> FilterTasksAsync(string status, DateTime? from = null, DateTime? to = null)
        {
            var cached = _taskCache.GetFilteredTasks(status, from, to);
            if (cached != null)
                return cached;

            var tareas = await _commonProcess.GetAllAsync();
            var filtered = tareas.Where(t =>
                (string.IsNullOrEmpty(status) || t.Status == status) &&
                (!from.HasValue || t.DueDate >= from.Value) &&
                (!to.HasValue || t.DueDate <= to.Value)
            ).ToList();

            _taskCache.SetFilteredTasks(status, from, to, filtered);
            return filtered;
        }

        // Limpia la caché cuando se modifica el conjunto de tareas
        private void ClearCaches()
        {
            _taskCache.ClearCompletionRate();
            _taskCache.ClearFilterCache();
        }
    }
}

