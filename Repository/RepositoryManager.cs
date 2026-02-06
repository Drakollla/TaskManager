using Domain.Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IWorkTaskRepository> _taskRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<ITagRepository> _tagRepository;

        public RepositoryManager(AppDbContext context)
        {
            _context = context;
            _taskRepository = new Lazy<IWorkTaskRepository>(() => new WorkTaskRepository(context));
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
            _tagRepository = new Lazy<ITagRepository>(() => new TagRepository(context));
        }

        public IWorkTaskRepository Task => _taskRepository.Value;
        public ICategoryRepository Category => _categoryRepository.Value;
        public ITagRepository Tag => _tagRepository.Value;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}