using Application.Features.Tags.Quaries;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.DTO;

namespace Application.Features.Tags.Handlers
{
    public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, TagDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetTagByIdHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TagDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _repository.Tag.GetTagByIdAsync(request.Id, request.UserId, request.TrackChanges);

            if (tag is null)
                throw new TagNotFoundException(request.Id);

            return _mapper.Map<TagDto>(tag);
        }
    }
}