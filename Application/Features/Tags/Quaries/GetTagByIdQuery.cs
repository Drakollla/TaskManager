using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Quaries
{
    public record GetTagByIdQuery(Guid Id, bool TrackChanges) : IRequest<TagDto>;
}