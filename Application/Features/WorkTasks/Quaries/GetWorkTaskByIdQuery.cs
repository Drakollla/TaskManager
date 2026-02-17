using MediatR;
using Shared.DTO;

namespace Application.Features.WorkTasks.Quaries
{
    public record GetWorkTaskByIdQuery(Guid Id, string UserId, bool TrackChanges) : IRequest<WorkTaskDto>;
}