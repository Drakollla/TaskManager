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
    }
}
