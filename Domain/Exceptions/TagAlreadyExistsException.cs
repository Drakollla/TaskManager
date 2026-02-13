namespace Domain.Exceptions
{
    public sealed class TagAlreadyExistsException : BadRequestException
    {
        public TagAlreadyExistsException(string name)
            : base($"Tag with name '{name}' already exists.") { }
    }
}
