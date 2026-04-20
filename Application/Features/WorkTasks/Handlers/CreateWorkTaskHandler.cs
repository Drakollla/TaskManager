using Application.Features.WorkTasks.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Shared.Mapping;

namespace Application.Features.WorkTasks.Handlers
{
    public class CreateWorkTaskHandler : IRequestHandler<CreateWorkTaskCommand, Guid>
    {
        private readonly IRepositoryManager _repository;

        public CreateWorkTaskHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateWorkTaskCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TaskDto;

            var category = await _repository.Category.GetCategoryByIdAsync(dto.CategoryId, request.UserId, trackChanges: false);

            if (category is null)
                throw new CategoryNotFoundException(dto.CategoryId);

            var taskEntity = WorkTaskMapper.ToEntity(dto);
            taskEntity.UserId = request.UserId;

            if (dto.TagIds != null && dto.TagIds.Any())
            {
                var tags = await _repository.Tag.GetTagsByIdsAsync(dto.TagIds, request.UserId, trackChanges: false);

                taskEntity.Tags = tags.ToList();
            }

            _repository.Task.CreateTask(taskEntity);
            await _repository.SaveAsync();

            return taskEntity.Id;
        }
    }
}
