using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Queries
{
    public record GetAllTagsQuery(string UserId, bool TrackChanges) : IRequest<IEnumerable<TagDto>>;
}