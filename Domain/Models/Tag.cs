using Domain.Models;

namespace TaskManager.Domain.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public ICollection<WorkTask>? Tasks { get; set; }
    }
}