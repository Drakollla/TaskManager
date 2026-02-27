using Application.Features.WorkTasks.Handlers;
using Application.Features.WorkTasks.Quaries;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using FluentAssertions;
using Moq;
using Shared.DTO;

namespace Tests.Features.WorkTasks
{
    public class GetWorkTaskByIdHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly GetWorkTaskByIdHandler _handler;

        public GetWorkTaskByIdHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();
            _mapperMock = new Mock<IMapper>();

            _repositoryManagerMock.Setup(x => x.Task)
                .Returns(_workTaskRepositoryMock.Object);

            _handler = new GetWorkTaskByIdHandler(_repositoryManagerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnDto_When_TaskExists()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var query = new GetWorkTaskByIdQuery(taskId, userId, false);

            var taskEntity = new WorkTask { Id = taskId, Title = "Db entity" };

            var expectedDto = new WorkTaskDto(
                 Id: taskId,
                 Title: "Test Task",
                 Description: "Test Desc",
                 CreatedAt: DateTime.Now,
                 DueDate: DateTime.Now.AddDays(1),
                 Priority: "High",
                 Status: "Todo",
                 CategoryName: "Work",
                 Tags: new List<TagDto>
                 {
                    new TagDto(Guid.NewGuid(), "Tag1", "#FF0000")
                 });

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, false))
                .ReturnsAsync(taskEntity);

            _mapperMock.Setup(x => x.Map<WorkTaskDto>(taskEntity))
                .Returns(expectedDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().Be(expectedDto);
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