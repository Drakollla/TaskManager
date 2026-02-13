using Application.DTO;
using Application.Features.Tags.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ISender _sender;

        public TagController(ISender sender) => _sender = sender;

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDto dto)
        {
            var command = new CreateTagCommand(dto);
            var tagId = await _sender.Send(command);

            return Ok(new { Id = tagId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] UpdateTagDto dto)
        {
            await _sender.Send(new UpdateTagCommand(id, dto));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            await _sender.Send(new DeleteTagCommand(id));
            return NoContent();
        }
    }
}