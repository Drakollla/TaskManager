using Application.Features.WorkTasks.Commands;
using Application.Features.WorkTasks.Quaries;
using Domain.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using System.Text.Json;
using TaskManagerAPI.Extensions;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    public class WorkTaskController : ControllerBase
    {
        private readonly ISender _sender;

        public WorkTaskController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] WorkTaskParameters parameters)
        {
            if (!parameters.ValidDateRange)
                return BadRequest("MaxDate cannot be less than MinDate");

            var userId = User.GetUserId();
            var result = await _sender.Send(new GetWorkTasksQuery(userId, parameters, TrackChanges: false));

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return Ok(result.tasks);
        }

        [HttpGet("{id:guid}", Name = "GetTaskById")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var userId = User.GetUserId(); 
            var task = await _sender.Send(new GetWorkTaskByIdQuery(id, userId, TrackChanges: false));

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateWorkTaskDto dto)
        {
            var userId = User.GetUserId();
            var taskId = await _sender.Send(new CreateWorkTaskCommand(userId, dto));

            return CreatedAtRoute("GetTaskById", new { id = taskId }, taskId);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateWorkTaskDto updateDto)
        {
            if (updateDto is null)
                return BadRequest("UpdateDto is null");

            var userId = User.GetUserId();
            await _sender.Send(new UpdateWorkTaskCommand(id, userId, updateDto));

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = User.GetUserId();
            await _sender.Send(new DeleteWorkTaskCommand(id, userId));

            return NoContent();
        }
    }
}