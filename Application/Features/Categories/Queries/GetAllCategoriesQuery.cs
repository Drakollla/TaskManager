using Application.DTO;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public record GetAllCategoriesQuery(bool TrackChanges) : IRequest<IEnumerable<CategoryDto>>;
}