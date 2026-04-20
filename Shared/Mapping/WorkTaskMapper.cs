using Domain.Models;
using Shared.DTO;

namespace Shared.Mapping
{
    public static class WorkTaskMapper
    {
        public static WorkTaskDto ToDto(WorkTask task) => new(
            task.Id,
            task.Title,
            task.Description,
            task.CreatedAt,
            task.DueDate,
            task.Priority.ToString(),
            task.Status.ToString(),
            task.Category?.Name ?? "Без категории",
            task.Tags?.Select(TagMapper.ToDto).ToList() ?? new List<TagDto>()
        );

        public static IEnumerable<WorkTaskDto> ToDto(IEnumerable<WorkTask> tasks) => tasks.Select(ToDto);

        public static WorkTask ToEntity(CreateWorkTaskDto dto) => new()
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            Status = dto.Status,
            CategoryId = dto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        public static void UpdateEntity(UpdateWorkTaskDto dto, WorkTask entity)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.DueDate = dto.DueDate;
            entity.Priority = dto.Priority;
            entity.Status = dto.Status;
            entity.CategoryId = dto.CategoryId;
        }
    }
}
