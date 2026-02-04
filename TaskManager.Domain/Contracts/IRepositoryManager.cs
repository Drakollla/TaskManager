namespace Domain.Contracts
{
    public interface IRepositoryManager
    {
        IWorkTaskRepository Task { get; }
        Task SaveAsync();
    }
}