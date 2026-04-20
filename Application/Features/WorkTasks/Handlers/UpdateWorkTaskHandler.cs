using Application.Features.WorkTasks.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.Mapping;

namespace Application.Features.WorkTasks.Handlers
{
    public class UpdateWorkTaskHandler : IRequestHandler<UpdateWorkTaskCommand, Unit>
    {
        private readonly IRepositoryManager _repository;

        public UpdateWorkTaskHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateWorkTaskCommand request, CancellationToken cancellationToken)
        {
            if (request.Id != request.UpdateDto.Id)
                throw new IdParametersBadRequestException(request.Id, request.UpdateDto.Id);

            var workTaskEntity = await _repository.Task.GetTaskByIdAsync(request.Id, request.UserId, trackChanges: true);

            if (workTaskEntity is null)
                throw new TaskNotFoundException(request.Id);

            var category = await _repository.Category.GetCategoryByIdAsync(request.UpdateDto.CategoryId, request.UserId, trackChanges: false);

            if (category is null)
                throw new CategoryNotFoundException(request.UpdateDto.CategoryId);

            WorkTaskMapper.UpdateEntity(request.UpdateDto, workTaskEntity);

            if (request.UpdateDto.TagIds != null)
            {
                var newTags = await _repository.Tag.GetTagsByIdsAsync(request.UpdateDto.TagIds, request.UserId, trackChanges: false);
                workTaskEntity.Tags = newTags.ToList();
            }
            else
            {
                workTaskEntity.Tags?.Clear();
            }

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
