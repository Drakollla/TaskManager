using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Models;

namespace Repository
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext appDbContext)
            : base(appDbContext) { }

        public void CreateTag(Tag tag) => Create(tag);

        public void DeleteTag(Tag tag) => Delete(tag);

        public async Task<IEnumerable<Tag>> GetAllTagsAsync(string userId, bool trackChanges) =>
            await FindAll(trackChanges)
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task<Tag?> GetTagByIdAsync(Guid id, string userId, bool trackChanges) =>
            await FindByCondition(x => x.Id.Equals(id) && x.UserId == userId, trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges)
                .ToListAsync();

        public async Task<Tag?> GetTagByNameAsync(string name, string userId, bool trackChanges) =>
            await FindByCondition(t => t.Name.ToLower() == name.ToLower() && t.UserId == userId, trackChanges)
                .SingleOrDefaultAsync();
    }
}