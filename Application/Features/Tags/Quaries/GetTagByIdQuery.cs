using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Quaries
{
    public record GetTagByIdQuery(Guid Id, string UserId, bool TrackChanges) : IRequest<TagDto>;
}