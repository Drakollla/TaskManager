using MediatR;
using Shared.DTO;

namespace Application.Features.Categories.Queries
{
    public record GetAllCategoriesQuery(bool TrackChanges) : IRequest<IEnumerable<CategoryDto>>;
}