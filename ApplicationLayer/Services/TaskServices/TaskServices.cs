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
                var result = await _commonProcess.AddAsync(tarea);
                response.Message = result.Message;
                response.Succesful = result.IsSuccess;
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
    }
}

