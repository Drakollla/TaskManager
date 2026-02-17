using MediatR;

namespace Application.Features.WorkTasks.Commands
{
    public record DeleteWorkTaskCommand(Guid Id, string UserId) : IRequest<Unit>;
}
