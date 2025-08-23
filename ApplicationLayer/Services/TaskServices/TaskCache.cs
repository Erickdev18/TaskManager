using System;
using System.Collections.Generic;
using DomainLayer.Models;

namespace ApplicationLayer.Services.TaskServices
{
    public class TaskCache
    {
        // Caché para porcentaje de tareas completadas
        private double? _completionRateCache;
        private DateTime _completionRateLastUpdate;

        // Caché para filtros de tareas
        private readonly Dictionary<(string status, DateTime? from, DateTime? to), List<Tareas>> _filterCache = new();

        // Guardar el porcentaje de tareas completadas
        public void SetCompletionRate(double rate)
        {
            _completionRateCache = rate;
            _completionRateLastUpdate = DateTime.Now;
        }

        // Obtener el porcentaje de tareas completadas
        public double? GetCompletionRate()
        {
            return _completionRateCache;
        }

        // Limpiar el caché de porcentaje
        public void ClearCompletionRate()
        {
            _completionRateCache = null;
        }

        // Guardar filtro de tareas
        public void SetFilteredTasks(string status, DateTime? from, DateTime? to, List<Tareas> tareas)
        {
            _filterCache[(status, from, to)] = tareas;
        }

        // Obtener filtro de tareas
        public List<Tareas>? GetFilteredTasks(string status, DateTime? from, DateTime? to)
        {
            _filterCache.TryGetValue((status, from, to), out var tareas);
            return tareas;
        }

        // Limpiar caché de filtros
        public void ClearFilterCache()
        {
            _filterCache.Clear();
        }
    }
}
