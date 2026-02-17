using Application.Features.Tags.Commands;
using Application.Features.Tags.Quaries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using TaskManagerAPI.Extensions;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tags")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ISender _sender;

        public TagController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var userId = User.GetUserId();
            var tags = await _sender.Send(new GetAllTagsQuery(userId, TrackChanges: false));
            return Ok(tags);
        }

        [HttpGet("{id:guid}", Name = "TagById")]
        public async Task<IActionResult> GetTag(Guid id)
        {
            var userId = User.GetUserId();
            var tag = await _sender.Send(new GetTagByIdQuery(id, userId, TrackChanges: false));

            if (tag is null)
                return NotFound();

            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDto dto)
        {
            var userId = User.GetUserId();
            var tagId = await _sender.Send(new CreateTagCommand(userId, dto));

            return CreatedAtRoute("GetTagById", new { id = tagId }, tagId);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] UpdateTagDto dto)
        {
            var userId = User.GetUserId();
            await _sender.Send(new UpdateTagCommand(id, userId, dto));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var userId = User.GetUserId();
            await _sender.Send(new DeleteTagCommand(id, userId));
            return NoContent();
        }
    }
}