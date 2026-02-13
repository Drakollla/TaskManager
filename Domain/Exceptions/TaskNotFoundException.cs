namespace Domain.Exceptions
{
    public sealed class TaskNotFoundException : NotFoundException
    {
        public TaskNotFoundException(Guid workTaskId)
            : base($"WorkTask with id: {workTaskId} doesn't exist in the database.") { }
    }
}