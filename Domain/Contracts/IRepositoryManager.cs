namespace Domain.Contracts
{
    public interface IRepositoryManager
    {
        IWorkTaskRepository Task { get; }
        ICategoryRepository Category { get; }
        ITagRepository Tag { get; }
        Task SaveAsync();
    }
}