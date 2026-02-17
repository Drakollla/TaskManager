using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Commands
{
    public record CreateTagCommand(CreateTagDto CreateTagDto) : IRequest<Guid>;
}
