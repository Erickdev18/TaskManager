using DomainLayer.Models;
using InfrastructureLayer.Context;
using InfrastructureLayer.Repository.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repository.TaskRepository
{
    public class TaskRepository : ICommonProcess<Tareas>
    {
        private readonly TaskManagerContext _context;

        public TaskRepository(TaskManagerContext taskManagerContext)
        {
            _context = taskManagerContext;
        }
        public async Task<IEnumerable<Tareas>> GetAllAsync()
            => await _context.Tarea.ToListAsync();

        public async Task<Tareas> GetByIdAsync(int id)
            => await _context.Tarea.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(bool IsSuccess, string Message)> AddAsync(Tareas entry)
        {
            try
            {
                await _context.Tarea.AddAsync(entry);
                await _context.SaveChangesAsync();
                return (true, "Tarea guardada correctamente.");
            }
            catch (Exception)
            {
                return (false, "Error al guardar la tarea.");
            }

        }
        public async Task<(bool IsSuccess, string Message)> UpdateAsync(Tareas entry)
        {
            try
            {
                _context.Tarea.Update(entry);
                await _context.SaveChangesAsync();
                return (true, "Tarea actualizada correctamente.");
            }
            catch (Exception)
            {
                return (false, "Error al actualizar la tarea.");
            }
        }
        public async Task<(bool IsSuccess, string Message)> DeleteAsync(int id)
        {
            try
            {
                var tarea = await _context.Tarea.FindAsync(id);
                if (tarea != null)
                {
                    _context.Tarea.Remove(tarea);
                    await _context.SaveChangesAsync();
                    return (true, "Tarea eliminada correctamente.");
                }
                else
                {
                    return (false, "Tarea no encontrada.");
                }
            }
            catch (Exception)
            {

                return (false, "Error al eliminar la tarea.");
            }

        }

    }
}
