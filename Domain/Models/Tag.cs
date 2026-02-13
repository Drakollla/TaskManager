namespace TaskManager.Domain.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }

        public ICollection<WorkTask>? Tasks { get; set; }
    }
}