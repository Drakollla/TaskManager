using Application.DTO;
using Application.Features.WorkTasks.Commands;
using Application.Features.WorkTasks.Quaries;
using Domain.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

            var result = await _sender.Send(new GetWorkTasksQuery(parameters, TrackChanges: false));
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return Ok(result.tasks);
        }

        [HttpGet("{id:guid}", Name = "GetTaskById")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var task = await _sender.Send(new GetWorkTaskByIdQuery(id, TrackChanges: false));

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateWorkTaskDto dto)
        {
            var taskId = await _sender.Send(new CreateWorkTaskCommand(dto));

            return CreatedAtRoute("GetTaskById", new { id = taskId }, taskId);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateWorkTaskDto updateDto)
        {
            if (updateDto is null)
                return BadRequest("UpdateDto is null");

            await _sender.Send(new UpdateWorkTaskCommand(id, updateDto));

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            await _sender.Send(new DeleteWorkTaskCommand(id));

            return NoContent();
        }
    }
}