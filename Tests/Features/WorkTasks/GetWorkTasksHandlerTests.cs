using Application.Features.WorkTasks.Handlers;
using Application.Features.WorkTasks.Queries;
using Domain.Contracts;
using Domain.Models;
using Domain.RequestFeatures;
using FluentAssertions;
using Moq;

namespace Tests.Features.WorkTasks
{
    public class GetWorkTasksHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;

        private readonly GetWorkTasksHandler _handler;

        public GetWorkTasksHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();

            _repositoryManagerMock.Setup(x => x.Task)
                .Returns(_workTaskRepositoryMock.Object);

            _handler = new GetWorkTasksHandler(_repositoryManagerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnPagedList_WhenCalled()
        {
            var userId = Guid.NewGuid().ToString();
            var query = new GetWorkTasksQuery(userId, new WorkTaskParameters(), false);

            var tasksFromDb = new List<WorkTask>
            {
                new WorkTask
                {
                    Id = Guid.NewGuid(),
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
                }
            };

            var pagedListFromDb = new PagedList<WorkTask>(tasksFromDb, 1, 1, 10);

            _workTaskRepositoryMock.Setup(x => x.GetAllTasksAsync(userId, It.IsAny<WorkTaskParameters>(), false))
                .ReturnsAsync(pagedListFromDb);

            var (tasks, metaData) = await _handler.Handle(query, CancellationToken.None);

            tasks.Should().NotBeNull();
            tasks.Should().HaveCount(1);
            tasks.First().Title.Should().Be("Test Task");
            metaData.Should().NotBeNull();
        }
    }
}