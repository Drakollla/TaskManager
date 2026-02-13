using TaskManager.Domain.Models;

namespace Domain.Contracts
{
    public interface IWorkTaskRepository
    {
        Task<IEnumerable<WorkTask>> GetAllTasksAsync(bool trackChanges);
        Task<WorkTask?> GetTaskByIdAsync(Guid id, bool trackChanges);
        Task<IEnumerable<WorkTask>> GetTaskByCategoryIdAsync(Guid categoryId, bool trackChanges);
        Task<IEnumerable<WorkTask>> GetTaskByTagIdAsync(Guid tagId, bool trackChanges);
        void CreateTask(WorkTask task);
        void DeleteTask(WorkTask task);
    }
}