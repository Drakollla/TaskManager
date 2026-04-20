using Application.Features.WorkTasks.Queries;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.DTO;
using Shared.Mapping;

namespace Application.Features.WorkTasks.Handlers
{
    public class GetWorkTaskByIdHandler : IRequestHandler<GetWorkTaskByIdQuery, WorkTaskDto>
    {
        private readonly IRepositoryManager _repository;

        public GetWorkTaskByIdHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<WorkTaskDto> Handle(GetWorkTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _repository.Task.GetTaskByIdAsync(request.Id, request.UserId, request.TrackChanges);

            if (task is null)
                throw new TaskNotFoundException(request.Id);

            return WorkTaskMapper.ToDto(task);
        }
    }
}