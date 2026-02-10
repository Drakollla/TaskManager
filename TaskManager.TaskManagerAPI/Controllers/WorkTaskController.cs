using Application.DTO;
using Application.Features.WorkTasks.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class WorkTaskController : ControllerBase
    {
        private readonly ISender _sender;

        public WorkTaskController(ISender sender) => _sender = sender;

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateWorkTaskDto dto)
        {
            var command = new CreateWorkTaskCommand(dto);
            var taskId = await _sender.Send(command);

            return Ok(new { Id = taskId });
        }
    }
}