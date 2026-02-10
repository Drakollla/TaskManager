using Application.DTO;
using Application.Features.Categories.Queries;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.Features.Categories.Handlers
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(request.Id, request.TrackChanges);

            // TODO: обработать ошибки позже
            if (category is null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
