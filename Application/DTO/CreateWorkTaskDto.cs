using TaskManager.Domain.Enums;

namespace Application.DTO
{
    public record CreateWorkTaskDto(
        string Title,
        string? Description,
        DateTime? DueDate,
        Priority Priority,
        TaskStatus Status,
        Guid CategoryId,     
        List<Guid> TagIds
    );
}
