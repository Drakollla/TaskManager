using Application.Features.Categories.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Shared.Mapping;

namespace Application.Features.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IRepositoryManager _repository;

        public CreateCategoryHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingCategory = await _repository.Category.GetCategoryByNameAsync(request.CreateCategoryDto.Name, request.UserId, trackChanges: false);

            if (existingCategory != null)
                throw new CategoryAlreadyExistsException(existingCategory.Name);

            var categoryEntity = CategoryMapper.ToEntity(request.CreateCategoryDto);
            categoryEntity.UserId = request.UserId;

            _repository.Category.CreateCategory(categoryEntity);

            await _repository.SaveAsync();

            return categoryEntity.Id;
        }
    }
}
