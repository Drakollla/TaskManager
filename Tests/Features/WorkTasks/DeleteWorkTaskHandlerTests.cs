using Application.Features.WorkTasks.Commands;
using Application.Features.WorkTasks.Handlers;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Tests.Features.WorkTasks
{
    public class DeleteWorkTaskHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;

        private readonly DeleteWorkTaskHandler _handler;

        public DeleteWorkTaskHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();

            _repositoryManagerMock.Setup(x => x.Task)
                .Returns(_workTaskRepositoryMock.Object);

            _handler = new DeleteWorkTaskHandler(_repositoryManagerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteTask_When_TaskExists()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var command = new DeleteWorkTaskCommand(taskId, userId);

            var existedTask = new WorkTask { Id = taskId, UserId = userId, Title = "To be deleted" };

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, false))
                .ReturnsAsync(existedTask);

            await _handler.Handle(command, CancellationToken.None);

            _workTaskRepositoryMock.Verify(x => x.DeleteTask(existedTask), Times.Once);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_TaskNotFound()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var command = new DeleteWorkTaskCommand(taskId, userId);

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, false))
                .ReturnsAsync((WorkTask?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<TaskNotFoundException>();

            _workTaskRepositoryMock.Verify(x => x.DeleteTask(It.IsAny<WorkTask>()), Times.Never);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}