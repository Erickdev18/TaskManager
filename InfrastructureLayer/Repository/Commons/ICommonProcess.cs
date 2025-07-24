using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repository.Commons
{
    public interface ICommonProcess<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(); //Obtiene todos los registros de la entidad T
        Task<T> GetByIdAsync(int id); //Obtiene un registro por su ID
        Task<(bool IsSuccess, string Message)> AddAsync(T entry);//Tupla de retorno para el resultado de la operación
        Task<(bool IsSuccess, string Message)> UpdateAsync(T entry); //Actualiza un registro existente
        Task<(bool IsSuccess, string Message)> DeleteAsync(int id); //Elimina un registro por su ID
    }
}
