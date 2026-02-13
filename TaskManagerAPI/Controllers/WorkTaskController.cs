using Application.DTO;
using Application.Features.WorkTasks.Commands;
using Application.Features.WorkTasks.Quaries;
using Domain.RequestFeatures;
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

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] WorkTaskParameters parameters)
        {
            if (!parameters.ValidDateRange)
                return BadRequest("MaxDate cannot be less than MinDate");

            var query = new GetWorkTasksQuery(parameters, TrackChanges: false);
            var tasks = await _sender.Send(query);

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateWorkTaskDto dto)
        {
            var command = new CreateWorkTaskCommand(dto);
            var taskId = await _sender.Send(command);

            return Ok(new { Id = taskId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateWorkTaskDto updateDto)
        {
            if (updateDto is null)
                return BadRequest("UpdateDto is null");

            var command = new UpdateWorkTaskCommand(id, updateDto);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var command = new DeleteWorkTaskCommand(id);
            await _sender.Send(command);

            return NoContent();
        }
    }
}