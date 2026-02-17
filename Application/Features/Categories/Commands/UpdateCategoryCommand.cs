using MediatR;
using Shared.DTO;

namespace Application.Features.Categories.Commands
{
    public record UpdateCategoryCommand(Guid Id, string UserId, UpdateCategoryDto Dto) : IRequest<Unit>;
}
