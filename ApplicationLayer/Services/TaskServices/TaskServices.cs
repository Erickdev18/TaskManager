using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repository.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.TaskServices
{
    public class TaskServices
    {
        private readonly ICommonProcess<Tareas> _commonProcess;
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

                var result = await _commonProcess.AddAsync(tarea);
                response.Message = result.Message;
                response.Succesful = result.IsSuccess;

                // Notificación con Action
                if (result.IsSuccess)
                    Notify?.Invoke($"Tarea creada: {tarea.Description}");

                // Cálculo de días restantes con Func
                if (result.IsSuccess)
                    response.Message += $" Días restantes: {DaysRemaining(tarea)}";
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }
        public async Task<Response<string>> UpdateTaskAllAsync(Tareas tarea)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonProcess.UpdateAsync(tarea);
                response.Message = result.Message;
                response.Succesful = result.IsSuccess;
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }
        public async Task<Response<string>> DeleteTaskAllAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonProcess.DeleteAsync(id);
                response.Message = result.Message;
                response.Succesful = result.IsSuccess;
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> AddHighPriorityTaskAsync(string description, string additionalData = "")
        {
            var tarea = DomainLayer.Models.TaskFactory.CreateHighPriorityTask(description, additionalData);
            return await AddTaskAllAsync(tarea);
        }
    }
}

