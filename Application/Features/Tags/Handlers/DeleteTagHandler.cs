using Application.Features.Tags.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Tags.Handlers
{
    public class DeleteTagHandler : IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly IRepositoryManager _repository;

        public DeleteTagHandler(IRepositoryManager repository) => _repository = repository;

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.Tag.GetTagByIdAsync(request.Id, trackChanges: false);

            if (tag is null)
                throw new TagNotFoundException(request.Id);

            _repository.Tag.DeleteTag(tag);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}