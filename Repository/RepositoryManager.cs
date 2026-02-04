using Domain.Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IWorkTaskRepository> _taskRepository;

        public RepositoryManager(AppDbContext context)
        {
            _context = context;
            _taskRepository = new Lazy<IWorkTaskRepository>(() => new WorkTaskRepository(context));
        }

        public IWorkTaskRepository Task => _taskRepository.Value;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}