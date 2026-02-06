using TaskManager.Domain.Models;

namespace Domain.Contracts
{
    public interface ITagRepository
    {
        void CreateTag(Tag tag);
        void DeleteTag(Tag tag);
        Task<IEnumerable<Tag>> GetAllTagsAsync(bool trackChanges);
        Task<Tag?> GetTagByIdAsync(Guid id, bool trackChanges);
    }
}