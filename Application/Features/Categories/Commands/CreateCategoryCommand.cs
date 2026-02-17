using MediatR;
using Shared.DTO;

namespace Application.Features.Categories.Commands
{
    public record CreateCategoryCommand(CreateCategoryDto CreateCategoryDto) : IRequest<Guid>;
}