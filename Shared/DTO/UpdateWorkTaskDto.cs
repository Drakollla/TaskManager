using Domain.Enums;

namespace Shared.DTO
{
    public record UpdateWorkTaskDto(Guid Id,
        string Title,
        string? Description,
        DateTime? DueDate,
        Priority Priority,
        WorkTaskStatus Status,
        Guid CategoryId,
        List<Guid> TagIds);
}