using Shared.Enums;

namespace Shared.DTO
{
    public record UpdateWorkTaskDto(Guid Id,
        string Title,
        string? Description,
        DateTime? DueDate,
        Priority Priority,
        TaskStatus Status,
        Guid CategoryId,
        List<Guid> TagIds);
}