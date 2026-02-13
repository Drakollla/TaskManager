namespace Domain.Exceptions
{
    public sealed class CategoryAlreadyExistsException : BadRequestException
    {
        public CategoryAlreadyExistsException(string name)
            : base($"Category with name '{name}' already exists.") { }
    }
}
