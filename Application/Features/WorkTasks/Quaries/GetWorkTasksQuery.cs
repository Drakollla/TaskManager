using Application.DTO;
using Domain.RequestFeatures;
using MediatR;

namespace Application.Features.WorkTasks.Quaries
{
    public record GetWorkTasksQuery(WorkTaskParameters Parameters, bool TrackChanges) : IRequest<IEnumerable<WorkTaskDto>>;
}