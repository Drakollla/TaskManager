using Application.Features.Tags.Commands;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Tags.Handlers
{
    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, Unit>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateTagHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.Tag.GetTagByIdAsync(request.Id, trackChanges: true);

            if (tag is null)
                throw new TagNotFoundException(request.Id);

            _mapper.Map(request.Dto, tag);

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
