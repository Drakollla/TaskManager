using MediatR;

namespace Application.Features.Tags.Commands
{
    public record DeleteTagCommand(Guid Id, string UserId) : IRequest<Unit>;
}