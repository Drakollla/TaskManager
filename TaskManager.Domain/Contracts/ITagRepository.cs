using TaskManager.Domain.Models;

namespace Domain.Contracts
{
    public interface ITagRepository
    {
        void CreateTag(Tag tag);
        void DeleteTag(Tag tag);
        Task<IEnumerable<Tag>> GetAllTagsAsync(bool trackChanges);
        Task<Tag?> GetTagByIdAsync(Guid id, bool trackChanges);
        Task<Tag?> GetTagByNameAsync(string name, bool trackChanges);
        Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}