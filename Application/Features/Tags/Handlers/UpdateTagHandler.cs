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

            if (!string.Equals(tag.Name, request.Dto.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                var duplicate = await _repository.Tag.GetTagByNameAsync(request.Dto.Name, trackChanges: false);
             
                if (duplicate != null)
                    throw new TagAlreadyExistsException(request.Dto.Name);
            }

            _mapper.Map(request.Dto, tag);

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
