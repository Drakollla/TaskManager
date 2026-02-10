using Application.Features.Tags.Commands;
using AutoMapper;
using Domain.Contracts;
using MediatR;
using TaskManager.Domain.Models;

namespace Application.Features.Tags.Handlers
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Guid>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateTagHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tagEntity = _mapper.Map<Tag>(request.CreateTagDto);

            _repository.Tag.CreateTag(tagEntity);

            await _repository.SaveAsync();

            return tagEntity.Id;
        }
    }
}
