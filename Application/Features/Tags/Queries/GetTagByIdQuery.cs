using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Queries
{
    public record GetTagByIdQuery(Guid Id, string UserId, bool TrackChanges) : IRequest<TagDto>;
}