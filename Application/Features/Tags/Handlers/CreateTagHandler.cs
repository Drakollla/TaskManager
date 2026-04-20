using Application.Features.Tags.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Shared.Mapping;

namespace Application.Features.Tags.Handlers
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Guid>
    {
        private readonly IRepositoryManager _repository;

        public CreateTagHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var existingTag = await _repository.Tag.GetTagByNameAsync(request.CreateTagDto.Name, request.UserId, trackChanges: false);

            if (existingTag != null)
                throw new TagAlreadyExistsException(existingTag.Name);

            var tagEntity = TagMapper.ToEntity(request.CreateTagDto);
            tagEntity.UserId = request.UserId;

            _repository.Tag.CreateTag(tagEntity);

            await _repository.SaveAsync();

            return tagEntity.Id;
        }
    }
}
