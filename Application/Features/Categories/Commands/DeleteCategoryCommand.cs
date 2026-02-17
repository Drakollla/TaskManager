using MediatR;

namespace Application.Features.Categories.Commands
{
    public record DeleteCategoryCommand(Guid Id, string UserId) : IRequest<Unit>;
}