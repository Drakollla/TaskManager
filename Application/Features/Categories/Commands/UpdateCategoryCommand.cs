using Application.DTO;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public record UpdateCategoryCommand(Guid Id, UpdateCategoryDto Dto) : IRequest<Unit>;
}
