using Application.Features.Tags.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;
using Shared.Mapping;

namespace Application.Features.Tags.Handlers
{
    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, Unit>
    {
        private readonly IRepositoryManager _repository;

        public UpdateTagHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.Tag.GetTagByIdAsync(request.Id, request.UserId, trackChanges: true);

            if (tag is null)
                throw new TagNotFoundException(request.Id);

            if (!string.Equals(tag.Name, request.Dto.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                var duplicate = await _repository.Tag.GetTagByNameAsync(request.Dto.Name, request.UserId, trackChanges: false);
              
                if (duplicate != null)
                    throw new TagAlreadyExistsException(request.Dto.Name);
            }

            TagMapper.UpdateEntity(request.Dto, tag);

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
