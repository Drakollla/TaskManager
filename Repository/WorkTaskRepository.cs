using Domain.Contracts;
using Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using TaskManager.Domain.Models;

namespace Repository
{
    public class WorkTaskRepository : RepositoryBase<WorkTask>, IWorkTaskRepository
    {
        public WorkTaskRepository(AppDbContext appDbContext)
            : base(appDbContext) { }

        public void CreateTask(WorkTask task) => Create(task);

        public void DeleteTask(WorkTask task) => Delete(task);

        public async Task<PagedList<WorkTask>> GetAllTasksAsync(string userId, WorkTaskParameters parameters, bool trackChanges)
        {
            var tasksQuery = FindAll(trackChanges)
                .Where(t => t.UserId == userId)
                .FilterWorkTasks(parameters.MinDate, parameters.MaxDate)
                .Search(parameters.SearchTerm)
                .Include(t => t.Category)
                .Include(t => t.Tags)
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Title)
                .AsNoTracking();

            return await tasksQuery.ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<WorkTask>> GetTaskByCategoryIdAsync(Guid categoryId, bool trackChanges) =>
            await FindByCondition(x => x.CategoryId.Equals(categoryId), trackChanges)
                .OrderBy(x => x.DueDate)
                .ToListAsync();

        public async Task<WorkTask?> GetTaskByIdAsync(Guid id, string userId, bool trackChanges) =>
            await FindByCondition(t => t.Id.Equals(id) && t.UserId == userId, trackChanges)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .AsSplitQuery()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<WorkTask>> GetTaskByTagIdAsync(Guid tagId, bool trackChanges) =>
            await FindByCondition(x => x.Tags.Any(tag => tag.Id == tagId), trackChanges)
                .OrderBy(x => x.DueDate)
                .ToListAsync();
    }
}