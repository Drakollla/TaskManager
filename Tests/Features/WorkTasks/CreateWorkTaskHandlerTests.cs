using Application.Features.WorkTasks.Commands;
using Application.Features.WorkTasks.Handlers;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using FluentAssertions;
using Moq;
using Shared.DTO;
using Shared.Enums;
using TaskManager.Domain.Models;

namespace Tests.Features.WorkTasks
{
    public class CreateWorkTaskHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly CreateWorkTaskHandler _handler;

        public CreateWorkTaskHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
            _mapperMock = new Mock<IMapper>();

            _repositoryManagerMock.Setup(x => x.Task).Returns(_workTaskRepositoryMock.Object);
            _repositoryManagerMock.Setup(x => x.Category).Returns(_categoryRepositoryMock.Object);
            _repositoryManagerMock.Setup(x => x.Tag).Returns(_tagRepositoryMock.Object);

            _handler = new CreateWorkTaskHandler(_repositoryManagerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnId_When_DataIsCorrect()
        {
            var userId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid();

            var dto = new CreateWorkTaskDto(
                Title: "Test Task",
                Description: "",
                DueDate: DateTime.Now,
                Priority: Priority.Low,
                Status: WorkTaskStatus.Todo,
                CategoryId: categoryId,
                TagIds: new List<Guid>());

            var command = new CreateWorkTaskCommand(userId, dto);

            _categoryRepositoryMock
                .Setup(x => x.GetCategoryByIdAsync(categoryId, userId, false))
                .ReturnsAsync(new Category { Id = categoryId, UserId = userId });

            var taskEntity = new WorkTask { Id = new Guid(), Title = dto.Title, UserId = userId };

            _mapperMock.Setup(m => m.Map<WorkTask>(dto))
                .Returns(taskEntity);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().Be(taskEntity.Id);

            _workTaskRepositoryMock.Verify(x => x.CreateTask(taskEntity), Times.Once);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouId_ThrowNotFound_When_CategoryDoesNotExist()
        {
            var userId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid();

            var dto = new CreateWorkTaskDto("Test", null, null, 0, 0, categoryId, new List<Guid>());
            var command = new CreateWorkTaskCommand(userId, dto);

            _categoryRepositoryMock
                .Setup(x => x.GetCategoryByIdAsync(categoryId, userId, false))
                .ReturnsAsync((Category?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<CategoryNotFoundException>();

            _workTaskRepositoryMock.Verify(x => x.CreateTask(It.IsAny<WorkTask>()), Times.Never);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}