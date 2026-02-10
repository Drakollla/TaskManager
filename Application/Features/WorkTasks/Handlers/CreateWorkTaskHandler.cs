using Application.Features.WorkTasks.Commands;
using AutoMapper;
using Domain.Contracts;
using MediatR;
using TaskManager.Domain.Models;

namespace Application.Features.WorkTasks.Handlers
{
    public class CreateWorkTaskHandler : IRequestHandler<CreateWorkTaskCommand, Guid>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateWorkTaskHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateWorkTaskCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TaskDto;

            var category = await _repository.Category.GetCategoryByIdAsync(dto.CategoryId, trackChanges: false);

            if (category is null)
                throw new Exception($"Category with id {dto.CategoryId} not found");

            var taskEntity = _mapper.Map<WorkTask>(dto);

            if (dto.TagIds != null && dto.TagIds.Any())
            {
                var tags = await _repository.Tag.GetTagsByIdsAsync(dto.TagIds, trackChanges: false);

                taskEntity.Tags = tags.ToList();
            }

            // 4. Сохраняем
            _repository.Task.CreateTask(taskEntity);
            await _repository.SaveAsync();

            return taskEntity.Id;
        }
    }
}
