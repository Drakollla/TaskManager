using Domain.Enums;

namespace Shared.DTO
{
    public record CreateWorkTaskDto(
        string Title,
        string? Description,
        DateTime? DueDate,
        Priority Priority,
        WorkTaskStatus Status,
        Guid CategoryId,
        List<Guid> TagIds
    );
}
