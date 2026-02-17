using TaskManager.Domain.Models;

namespace Domain.Contracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(string userId, bool trackChanges);
        Task<Category?> GetCategoryByIdAsync(Guid id, string userId, bool trackChanges);
        Task<Category?> GetCategoryByNameAsync(string name, string userId, bool trackChanges);
        void CreateCategory(Category category);
        void DeleteCategory(Category category);
    }
}