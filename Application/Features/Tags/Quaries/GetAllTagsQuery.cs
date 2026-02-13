using Application.DTO;
using MediatR;

namespace Application.Features.Tags.Quaries
{
    public record GetAllTagsQuery(bool TrackChanges) : IRequest<IEnumerable<TagDto>>;
}