using MediatR;
using Shared.DTO;

namespace Application.Features.Categories.Queries
{
    public record GetCategoryByIdQuery(Guid Id, bool TrackChanges) : IRequest<CategoryDto>;
}