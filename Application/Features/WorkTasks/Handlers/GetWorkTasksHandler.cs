using Application.Features.WorkTasks.Queries;
using Domain.Contracts;
using Domain.RequestFeatures;
using MediatR;
using Shared.DTO;
using Shared.Mapping;

namespace Application.Features.WorkTasks.Handlers
{
    public class GetWorkTasksHandler : IRequestHandler<GetWorkTasksQuery, (IEnumerable<WorkTaskDto> tasks, MetaData metaData)>
    {
        private readonly IRepositoryManager _repository;

        public GetWorkTasksHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<WorkTaskDto> tasks, MetaData metaData)> Handle(GetWorkTasksQuery request, CancellationToken cancellationToken)
        {
            var tasksWithMetadata = await _repository.Task.GetAllTasksAsync(request.UserId, request.Parameters, request.TrackChanges);
            var tasksDto = WorkTaskMapper.ToDto(tasksWithMetadata);

            return (tasksDto, tasksWithMetadata.MetaData);
        }
    }
}