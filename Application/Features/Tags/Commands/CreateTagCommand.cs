using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Commands
{
    public record CreateTagCommand(string UserId, CreateTagDto CreateTagDto) : IRequest<Guid>;
}
