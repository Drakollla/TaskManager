using Application.DTO;
using MediatR;

namespace Application.Features.WorkTasks.Quaries
{
    public record GetWorkTaskByIdQuery(Guid Id, bool TrackChanges) : IRequest<WorkTaskDto>;
}