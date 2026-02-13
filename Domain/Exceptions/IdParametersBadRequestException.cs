namespace Domain.Exceptions
{
    public sealed class IdParametersBadRequestException : BadRequestException
    {
        public IdParametersBadRequestException(Guid idFromRoute, Guid idFromDto)
            : base($"Parameter id: {idFromRoute} and DTO id: {idFromDto} do not match.")
        {
        }
    }
}