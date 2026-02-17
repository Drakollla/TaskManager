using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Quaries
{
    public record GetAllTagsQuery(bool TrackChanges) : IRequest<IEnumerable<TagDto>>;
}