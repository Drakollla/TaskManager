using Application.DTO;
using MediatR;

namespace Application.Features.WorkTasks.Commands
{
    public record UpdateWorkTaskCommand(Guid Id, UpdateWorkTaskDto UpdateDto) : IRequest<Unit>;
}