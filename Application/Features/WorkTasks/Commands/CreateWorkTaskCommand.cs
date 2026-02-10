using Application.DTO;
using MediatR;

namespace Application.Features.WorkTasks.Commands
{
    public record CreateWorkTaskCommand(CreateWorkTaskDto TaskDto) : IRequest<Guid>;
}