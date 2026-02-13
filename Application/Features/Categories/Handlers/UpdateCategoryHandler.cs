using Application.Features.Categories.Commands;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Handlers
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(request.Id, trackChanges: true);

            if (category is null)
                throw new CategoryNotFoundException(request.Id);

            _mapper.Map(request.Dto, category);

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}