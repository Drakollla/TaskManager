using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Models;

namespace Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext appDbContext)
            : base(appDbContext) { }

        public void CreateCategory(Category category) => Create(category);

        public void DeleteCategory(Category category) => Delete(category);

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string userId, bool trackChanges) =>
            await FindAll(trackChanges)
                .Where(c=>c.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task<Category?> GetCategoryByIdAsync(Guid id, string userId, bool trackChanges) =>
            await FindByCondition(x => x.Id.Equals(id) && x.UserId == userId, trackChanges)
                .SingleOrDefaultAsync();

        public async Task<Category?> GetCategoryByNameAsync(string name, string userId, bool trackChanges) =>
            await FindByCondition(c => c.Name.ToLower() == name.ToLower() && c.UserId == userId, trackChanges)
                .SingleOrDefaultAsync();
    }
}