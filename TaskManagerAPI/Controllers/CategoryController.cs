using Application.DTO;
using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ISender _sender;

        public CategoryController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _sender.Send(new GetAllCategoriesQuery(TrackChanges: false));

            return Ok(categories);
        }

        [HttpGet("{id:guid}", Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _sender.Send(new GetCategoryByIdQuery(id, TrackChanges: false));

            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto is null)
                return BadRequest("CategoryDto object is null");

            var createdCategoryId = await _sender.Send(new CreateCategoryCommand(createCategoryDto));

            return CreatedAtRoute("CategoryById", new { id = createdCategoryId }, new { id = createdCategoryId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto)
        {
            await _sender.Send(new UpdateCategoryCommand(id, dto));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _sender.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}