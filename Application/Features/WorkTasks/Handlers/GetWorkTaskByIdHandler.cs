using Application.Features.WorkTasks.Quaries;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.DTO;

namespace Application.Features.WorkTasks.Handlers
{
    public class GetWorkTaskByIdHandler : IRequestHandler<GetWorkTaskByIdQuery, WorkTaskDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetWorkTaskByIdHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WorkTaskDto> Handle(GetWorkTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _repository.Task.GetTaskByIdAsync(request.Id, request.UserId, request.TrackChanges);

            if (task is null)
                throw new TaskNotFoundException(request.Id);

            var dto = _mapper.Map<WorkTaskDto>(task);

            return dto;
        }
    }
}