using ApplicationLayer.Services.TaskServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TaskServices _service;
        public TareasController(TaskServices service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task <ActionResult<Response<Tareas>>> GetTaskAllAsync()
            => await _service.GetTaskAllAsync();
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Tareas>>> GetTaskByIdAllAsync(int id)
            => await _service.GetTaskByIdAllAsync(id);
        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddTaskAllAsync(Tareas tarea)
            => await _service.AddTaskAllAsync(tarea);
        [HttpPut]
        public async Task<ActionResult<Response<string>>> UpdateTaskAllAsync(Tareas tarea)
            => await _service.UpdateTaskAllAsync(tarea);
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAllAsync(int id)
            => await _service.DeleteTaskAllAsync(id);
        [HttpPost("alta-prioridad")]
        public async Task<ActionResult<Response<string>>> AddHighPriorityTask([FromBody] string descripcion)
        {
            var tarea = DomainLayer.Models.TaskFactory.CreateHighPriorityTask(descripcion);
            return await _service.AddTaskAllAsync(tarea);
        }

        [HttpPost("completada")]
        public async Task<ActionResult<Response<string>>> AddCompletedTask([FromBody] Tareas tareaDto)
        {
            var tarea = DomainLayer.Models.TaskFactory.CreateCompletedTask(
                tareaDto.Description,
                tareaDto.DueDate,
                tareaDto.AdditionalData
            );
            return await _service.AddTaskAllAsync(tarea);
        }

        [HttpPost("personalizada")]
        public async Task<ActionResult<Response<string>>> AddCustomTask([FromBody] Tareas tareaDto)
        {
            var tarea = DomainLayer.Models.TaskFactory.CreateCustomTask(
                tareaDto.Description,
                tareaDto.DueDate,
                tareaDto.Status,
                tareaDto.AdditionalData
            );
            return await _service.AddTaskAllAsync(tarea);
        }
    }
    
}
