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

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task<Category?> GetCategoryByIdAsync(Guid id, bool trackChanges) =>
            await FindByCondition(x => x.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<Category?> GetCategoryByNameAsync(string name, bool trackChanges) =>
            await FindByCondition(c => c.Name.ToLower() == name.ToLower(), trackChanges)
                .SingleOrDefaultAsync();
    }
}