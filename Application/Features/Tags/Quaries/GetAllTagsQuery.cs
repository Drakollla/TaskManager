using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Quaries
{
    public record GetAllTagsQuery(string UserId, bool TrackChanges) : IRequest<IEnumerable<TagDto>>;
}