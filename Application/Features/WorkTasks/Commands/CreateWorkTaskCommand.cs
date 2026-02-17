using MediatR;
using Shared.DTO;

namespace Application.Features.WorkTasks.Commands
{
    public record CreateWorkTaskCommand(string UserId, CreateWorkTaskDto TaskDto) : IRequest<Guid>;
}