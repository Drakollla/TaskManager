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
            var query = new GetCategoryByIdQuery(id, TrackChanges: false);
            var category = await _sender.Send(query);

            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto is null)
                return BadRequest("CategoryDto object is null");

            var command = new CreateCategoryCommand(createCategoryDto);
            var createdCategoryId = await _sender.Send(command);

            return CreatedAtRoute("CategoryById", new { id = createdCategoryId }, new { id = createdCategoryId });
        }
    }
}
