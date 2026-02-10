namespace Domain.Exceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(Guid categoryId)
            : base($"Category with id: {categoryId} doesn't exist in the database.") { }
    }
}
