using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Commands
{
    public record UpdateTagCommand(Guid Id, UpdateTagDto Dto) : IRequest<Unit>;
}