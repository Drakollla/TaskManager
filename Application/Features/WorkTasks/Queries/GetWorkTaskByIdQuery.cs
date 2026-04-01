using MediatR;
using Shared.DTO;

namespace Application.Features.WorkTasks.Queries
{
    public record GetWorkTaskByIdQuery(Guid Id, string UserId, bool TrackChanges) : IRequest<WorkTaskDto>;
}