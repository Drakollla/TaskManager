using MediatR;
using Shared.DTO;

namespace Application.Features.Categories.Commands
{
    public record UpdateCategoryCommand(Guid Id, UpdateCategoryDto Dto) : IRequest<Unit>;
}
