using MediatR;
using Shared.DTO;

namespace Application.Features.Categories.Queries
{
    public record GetCategoryByIdQuery(Guid Id, string UserId, bool TrackChanges) : IRequest<CategoryDto>;
}