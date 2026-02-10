using Application.DTO;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public record CreateCategoryCommand(CreateCategoryDto CreateCategoryDto) : IRequest<Guid>;
}