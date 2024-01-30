using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Dto.Input;
using TaskManagement.Domain.Dto.Output;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.API.Task.Controllers
{
    [ApiController, Route("api/[controller]"), EnableCors("General")]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public PaginationOutput<TaskOutput> Get([FromQuery]TaskReadInput input)
        {
            return _taskService.Get(input);
        }

        [HttpPost]
        public bool Create([FromBody] TaskInput input)
        {
            return _taskService.Create(input);
        }

        [HttpPut]
        public bool Update([FromBody] TaskInput input)
        {
            return _taskService.Update(input);
        }

        [HttpDelete, Route("{id?}")]
        public bool Delete([FromRoute] string id)
        {
            Guid.TryParse(id, out Guid guid);
            return _taskService.Delete(guid);
        }
    }
}
