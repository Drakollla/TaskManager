using Application.Features.Categories.Queries;
using Domain.Contracts;
using MediatR;
using Shared.DTO;
using Shared.Mapping;

namespace Application.Features.Categories.Handlers
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IRepositoryManager _repository;

        public GetAllCategoriesHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repository.Category.GetAllCategoriesAsync(request.UserId, request.TrackChanges);
            return CategoryMapper.ToDto(categories);
        }
    }
}