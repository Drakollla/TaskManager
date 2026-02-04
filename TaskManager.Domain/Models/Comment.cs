namespace TaskManager.Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid TaskId { get; set; }
        public WorkTask? Task { get; set; }
    }
}