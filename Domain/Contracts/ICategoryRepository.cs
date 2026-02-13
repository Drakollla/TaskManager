using TaskManager.Domain.Models;

namespace Domain.Contracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
        Task<Category?> GetCategoryByIdAsync(Guid id, bool trackChanges);
        Task<Category?> GetCategoryByNameAsync(string name, bool trackChanges);
        void CreateCategory(Category category);
        void DeleteCategory(Category category);
    }
}