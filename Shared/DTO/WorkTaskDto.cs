namespace Shared.DTO
{
    public record WorkTaskDto(
        Guid Id,
        string Title,
        string? Description,
        DateTime CreatedAt,
        DateTime? DueDate,
        string Priority,
        string Status,
        string CategoryName,
        List<TagDto> Tags
    );
}