using Application.DTO;
using Application.Features.WorkTasks.Quaries;
using AutoMapper;
using Domain.Contracts;
using Domain.RequestFeatures;
using MediatR;

namespace Application.Features.WorkTasks.Handlers
{
    public class GetWorkTasksHandler : IRequestHandler<GetWorkTasksQuery, (IEnumerable<WorkTaskDto> tasks, MetaData metaData)>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetWorkTasksHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<WorkTaskDto> tasks, MetaData metaData)> Handle(GetWorkTasksQuery request, CancellationToken cancellationToken)
        {
            var tasksWithMetadata = await _repository.Task.GetAllTasksAsync(request.Parameters, request.TrackChanges);
            var tasksDto = _mapper.Map<IEnumerable<WorkTaskDto>>(tasksWithMetadata);

            return (tasksDto, tasksWithMetadata.MetaData);
        }
    }
}