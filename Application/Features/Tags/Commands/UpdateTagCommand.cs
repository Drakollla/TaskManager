using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Commands
{
    public record UpdateTagCommand(Guid Id, string UserId, UpdateTagDto Dto) : IRequest<Unit>;
}