using Domain.Enums;
using TaskManager.Domain.Models;

namespace Domain.Models
{
    public class WorkTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public WorkTaskStatus Status { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}