namespace TaskManager.Domain.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<WorkTask>? Tasks { get; set; }
    }
}