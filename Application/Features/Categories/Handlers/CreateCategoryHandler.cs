using Application.Features.Categories.Commands;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using TaskManager.Domain.Models;

namespace Application.Features.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingCategory = await _repository.Category.GetCategoryByNameAsync(request.CreateCategoryDto.Name, trackChanges: false);

            if (existingCategory != null)
                throw new CategoryAlreadyExistsException(existingCategory.Name);

            var categoryEntity = _mapper.Map<Category>(request.CreateCategoryDto);

            _repository.Category.CreateCategory(categoryEntity);

            await _repository.SaveAsync();

            return categoryEntity.Id;
        }
    }
}
