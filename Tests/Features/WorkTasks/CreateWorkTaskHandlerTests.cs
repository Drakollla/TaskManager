using Application.Features.WorkTasks.Commands;
using Application.Features.WorkTasks.Handlers;
using Domain.Contracts;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using FluentAssertions;
using Moq;
using Shared.DTO;

namespace Tests.Features.WorkTasks
{
    public class CreateWorkTaskHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;

        private readonly CreateWorkTaskHandler _handler;

        public CreateWorkTaskHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();

            _repositoryManagerMock.Setup(x => x.Task).Returns(_workTaskRepositoryMock.Object);
            _repositoryManagerMock.Setup(x => x.Category).Returns(_categoryRepositoryMock.Object);
            _repositoryManagerMock.Setup(x => x.Tag).Returns(_tagRepositoryMock.Object);

            _handler = new CreateWorkTaskHandler(_repositoryManagerMock.Object);
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

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().Be(Guid.Empty);

            _workTaskRepositoryMock.Verify(x => x.CreateTask(It.Is<WorkTask>(t => t.Title == "Test Task")), Times.Once);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFound_When_CategoryDoesNotExist()
        {
            var userId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid();

            var dto = new CreateWorkTaskDto("Test", null, null, Priority.Low, WorkTaskStatus.Todo, categoryId, new List<Guid>());
            var command = new CreateWorkTaskCommand(userId, dto);

            _categoryRepositoryMock
                .Setup(x => x.GetCategoryByIdAsync(categoryId, userId, false))
                .ReturnsAsync((Category?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<CategoryNotFoundException>();

            _workTaskRepositoryMock.Verify(x => x.CreateTask(It.IsAny<WorkTask>()), Times.Never);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_AttachTags_When_TagsProvided()
        {
            var userId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid();

            var tagId1 = Guid.NewGuid();
            var tagId2 = Guid.NewGuid();
            var tagIds = new List<Guid>() { tagId1, tagId2 };

            var existingTags = new List<Tag>
            {
                new Tag { Id = tagId1, Name = "Tag 1" },
                new Tag { Id = tagId2, Name = "Tag 2" },
            };

            var dto = new CreateWorkTaskDto("Task with Tags", "", DateTime.Now, Priority.High, WorkTaskStatus.Todo, categoryId, tagIds);
            var command = new CreateWorkTaskCommand(userId, dto);

            _categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(categoryId, userId, false))
                .ReturnsAsync(new Category { Id = categoryId });

            _tagRepositoryMock.Setup(x => x.GetTagsByIdsAsync(tagIds, userId, false))
                .ReturnsAsync(existingTags);

            await _handler.Handle(command, CancellationToken.None);

            _tagRepositoryMock.Verify(x => x.GetTagsByIdsAsync(tagIds, userId, false), Times.Once);
            _workTaskRepositoryMock.Verify(x => x.CreateTask(It.Is<WorkTask>(t => t.Tags != null && t.Tags.Count() == 2)), Times.Once);
        }
    }
}