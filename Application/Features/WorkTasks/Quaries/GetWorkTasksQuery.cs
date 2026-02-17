using Domain.RequestFeatures;
using MediatR;
using Shared.DTO;

namespace Application.Features.WorkTasks.Quaries
{
    public record GetWorkTasksQuery(string UserId, WorkTaskParameters Parameters, bool TrackChanges) : IRequest<(IEnumerable<WorkTaskDto> tasks, MetaData metaData)>;
}