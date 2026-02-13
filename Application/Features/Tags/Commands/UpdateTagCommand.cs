using Application.DTO;
using MediatR;

namespace Application.Features.Tags.Commands
{
    public record UpdateTagCommand(Guid Id, UpdateTagDto Dto) : IRequest<Unit>;
}