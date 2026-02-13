using Application.Features.WorkTasks.Commands;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.WorkTasks.Handlers
{
    public class UpdateWorkTaskHandler : IRequestHandler<UpdateWorkTaskCommand, Unit>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateWorkTaskHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateWorkTaskCommand request, CancellationToken cancellationToken)
        {
            if (request.Id != request.UpdateDto.Id)
                throw new IdParametersBadRequestException(request.Id, request.UpdateDto.Id);

            var workTaskEntity = await _repository.Task.GetTaskByIdAsync(request.Id, trackChanges: true);

            if (workTaskEntity is null)
                throw new TaskNotFoundException(request.Id);

            _mapper.Map(request.UpdateDto, workTaskEntity);

            if (request.UpdateDto.TagIds != null)
            {
                var newTags = await _repository.Tag.GetTagsByIdsAsync(request.UpdateDto.TagIds, trackChanges: false);
                workTaskEntity.Tags = newTags.ToList();
            }
            else
            {
                workTaskEntity.Tags.Clear();
            }

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
