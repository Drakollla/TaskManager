namespace Domain.Exceptions
{
    public sealed class TagNotFoundException : NotFoundException
    {
        public TagNotFoundException(Guid tagId)
            : base($"Tag with id: {tagId} doesn't exist in the database.") { }
    }
}