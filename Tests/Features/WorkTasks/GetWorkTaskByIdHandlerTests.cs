using Application.Features.WorkTasks.Handlers;
using Application.Features.WorkTasks.Queries;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Tests.Features.WorkTasks
{
    public class GetWorkTaskByIdHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;

        private readonly GetWorkTaskByIdHandler _handler;

        public GetWorkTaskByIdHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();

            _repositoryManagerMock.Setup(x => x.Task)
                .Returns(_workTaskRepositoryMock.Object);

            _handler = new GetWorkTaskByIdHandler(_repositoryManagerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnDto_When_TaskExists()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var query = new GetWorkTaskByIdQuery(taskId, userId, false);

            var taskEntity = new WorkTask
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Desc",
                CreatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                Priority = Domain.Enums.Priority.High,
                Status = Domain.Enums.WorkTaskStatus.Todo,
                Category = new Category { Name = "Work" },
                Tags = new List<Tag>
                {
                    new Tag { Id = Guid.NewGuid(), Name = "Tag1", ColorHex = "#FF0000" }
                }
            };

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, false))
                .ReturnsAsync(taskEntity);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(taskId);
            result.Title.Should().Be("Test Task");
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_TaskNotFound()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var query = new GetWorkTaskByIdQuery(taskId, userId, false);

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, false))
                .ReturnsAsync((WorkTask?)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<TaskNotFoundException>();
        }
    }
}