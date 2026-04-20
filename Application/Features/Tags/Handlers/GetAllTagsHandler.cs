using Application.Features.Tags.Queries;
using Domain.Contracts;
using MediatR;
using Shared.DTO;
using Shared.Mapping;

namespace Application.Features.Tags.Handlers
{
    public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, IEnumerable<TagDto>>
    {
        private readonly IRepositoryManager _repository;

        public GetAllTagsHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _repository.Tag.GetAllTagsAsync(request.UserId, request.TrackChanges);
            return TagMapper.ToDto(tags);
        }
    }
}