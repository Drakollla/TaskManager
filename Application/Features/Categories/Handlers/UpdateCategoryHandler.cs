using Application.Features.Categories.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.Mapping;

namespace Application.Features.Categories.Handlers
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IRepositoryManager _repository;

        public UpdateCategoryHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(request.Id, request.UserId, trackChanges: true);

            if (category is null)
                throw new CategoryNotFoundException(request.Id);

            var existingCategory = await _repository.Category.GetCategoryByNameAsync(
                request.UpdateCategoryDto.Name, request.UserId, trackChanges: false);

            if (existingCategory != null && existingCategory.Id != request.Id)
                throw new CategoryAlreadyExistsException(request.UpdateCategoryDto.Name);

            CategoryMapper.UpdateEntity(request.UpdateCategoryDto, category);

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}