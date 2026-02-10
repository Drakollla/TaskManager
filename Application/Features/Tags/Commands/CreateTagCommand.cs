using Application.DTO;
using MediatR;

namespace Application.Features.Tags.Commands
{
    public record CreateTagCommand(CreateTagDto CreateTagDto) : IRequest<Guid>;
}
