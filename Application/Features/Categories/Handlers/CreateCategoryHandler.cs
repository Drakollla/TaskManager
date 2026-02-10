using Application.Features.Categories.Commands;
using AutoMapper;
using Domain.Contracts;
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
            var categoryEntity = _mapper.Map<Category>(request.CreateCategoryDto);

            _repository.Category.CreateCategory(categoryEntity);

            await _repository.SaveAsync();

            return categoryEntity.Id;
        }
    }
}
