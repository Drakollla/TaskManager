using Application.DTO;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public record GetCategoryByIdQuery(Guid Id, bool TrackChanges) : IRequest<CategoryDto>;
}