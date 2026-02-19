using Domain.Models;
using Domain.RequestFeatures;

namespace Domain.Contracts
{
    public interface IWorkTaskRepository
    {
        Task<PagedList<WorkTask>> GetAllTasksAsync(string userId, WorkTaskParameters parameters, bool trackChanges);
        Task<WorkTask?> GetTaskByIdAsync(Guid id, string userId, bool trackChanges);
        Task<IEnumerable<WorkTask>> GetTaskByCategoryIdAsync(Guid categoryId, bool trackChanges);
        Task<IEnumerable<WorkTask>> GetTaskByTagIdAsync(Guid tagId, bool trackChanges);
        void CreateTask(WorkTask task);
        void DeleteTask(WorkTask task);
    }
}