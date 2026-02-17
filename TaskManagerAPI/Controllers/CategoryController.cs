using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using TaskManagerAPI.Extensions;

namespace TaskManagerAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ISender _sender;

        public CategoryController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var userId = User.GetUserId();
            var categories = await _sender.Send(new GetAllCategoriesQuery(userId, TrackChanges: false));

            return Ok(categories);
        }

        [HttpGet("{id:guid}", Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var userId = User.GetUserId();
            var category = await _sender.Send(new GetCategoryByIdQuery(id, userId, TrackChanges: false));

            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto is null)
                return BadRequest("CategoryDto object is null");

            var userId = User.GetUserId();
            var createdCategoryId = await _sender.Send(new CreateCategoryCommand(userId, createCategoryDto));

            return CreatedAtRoute("CategoryById", new { id = createdCategoryId }, new { id = createdCategoryId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto)
        {
            var userId = User.GetUserId();
            await _sender.Send(new UpdateCategoryCommand(id, userId, dto));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var userId = User.GetUserId();
            await _sender.Send(new DeleteCategoryCommand(id, userId));
            return NoContent();
        }
    }
}