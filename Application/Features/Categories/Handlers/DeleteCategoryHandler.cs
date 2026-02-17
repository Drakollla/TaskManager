using Application.Features.Categories.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IRepositoryManager _repository;

        public DeleteCategoryHandler(IRepositoryManager repository) => _repository = repository;

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(request.Id, request.UserId, trackChanges: false);

            if (category is null)
                throw new CategoryNotFoundException(request.Id);

            _repository.Category.DeleteCategory(category);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}