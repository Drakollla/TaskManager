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
    }
}