using MediatR;
using Shared.DTO;

namespace Application.Features.Categories.Commands
{
    public record CreateCategoryCommand(string UserId, CreateCategoryDto CreateCategoryDto) : IRequest<Guid>;
}