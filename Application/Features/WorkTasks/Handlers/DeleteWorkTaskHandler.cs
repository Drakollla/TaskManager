using Application.Features.WorkTasks.Commands;
using Domain.Contracts;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.WorkTasks.Handlers
{
    public class DeleteWorkTaskHandler : IRequestHandler<DeleteWorkTaskCommand, Unit>
    {
        private readonly IRepositoryManager _repository;

        public DeleteWorkTaskHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteWorkTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.Task.GetTaskByIdAsync(request.Id, trackChanges: false);

            if (task is null)
                throw new TaskNotFoundException(request.Id);

            _repository.Task.DeleteTask(task);

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}