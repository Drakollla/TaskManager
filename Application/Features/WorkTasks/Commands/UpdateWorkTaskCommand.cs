using MediatR;
using Shared.DTO;

namespace Application.Features.WorkTasks.Commands
{
    public record UpdateWorkTaskCommand(Guid Id, string UserId, UpdateWorkTaskDto UpdateDto) : IRequest<Unit>;
}