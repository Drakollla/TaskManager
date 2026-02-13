using Application.DTO;
using MediatR;

namespace Application.Features.Tags.Quaries
{
    public record GetTagByIdQuery(Guid Id, bool TrackChanges) : IRequest<TagDto>;
}