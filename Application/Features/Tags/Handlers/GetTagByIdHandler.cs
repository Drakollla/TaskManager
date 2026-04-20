using Application.Features.Tags.Queries;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.DTO;
using Shared.Mapping;

namespace Application.Features.Tags.Handlers
{
    public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, TagDto>
    {
        private readonly IRepositoryManager _repository;

        public GetTagByIdHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<TagDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _repository.Tag.GetTagByIdAsync(request.Id, request.UserId, request.TrackChanges);

            if (tag is null)
                throw new TagNotFoundException(request.Id);

            return TagMapper.ToDto(tag);
        }
    }
}