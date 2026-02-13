using Application.DTO;
using Application.Features.WorkTasks.Quaries;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.Features.WorkTasks.Handlers
{
    public class GetWorkTasksHandler : IRequestHandler<GetWorkTasksQuery, IEnumerable<WorkTaskDto>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetWorkTasksHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkTaskDto>> Handle(GetWorkTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _repository.Task.GetAllTasksAsync(request.TrackChanges);
            var tasksDto = _mapper.Map<IEnumerable<WorkTaskDto>>(tasks);

            return tasksDto;
        }
    }
}