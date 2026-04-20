using Application.Features.Categories.Queries;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.DTO;
using Shared.Mapping;

namespace Application.Features.Categories.Handlers
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IRepositoryManager _repository;

        public GetCategoryByIdHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(request.Id, request.UserId, request.TrackChanges);

            if (category is null)
                throw new CategoryNotFoundException(request.Id);

            return CategoryMapper.ToDto(category);
        }
    }
}