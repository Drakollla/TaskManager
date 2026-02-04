using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Models;

namespace Repository
{
    public class WorkTaskRepository : RepositoryBase<WorkTask>, IWorkTaskRepository
    {
        public WorkTaskRepository(AppDbContext appDbContext) 
            : base(appDbContext)
        {
        }

        public void CreateTask(WorkTask task) => Create(task);

        public void DeleteTask(WorkTask task) => Delete(task);

        public async Task<IEnumerable<WorkTask>> GetAllTasksAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(t => t.Title)
                .ToListAsync();

        public async Task<WorkTask?> GetTaskByIdAsync(Guid id, bool trackChanges) =>
            await FindByCondition(t => t.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();
    }
}
