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
    public class UpdateWorkTaskHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

        private readonly UpdateWorkTaskHandler _handler;

        public UpdateWorkTaskHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();

            _repositoryManagerMock.Setup(x => x.Task)
                .Returns(_workTaskRepositoryMock.Object);

            _repositoryManagerMock.SetupGet(x => x.Tag)
                .Returns(_tagRepositoryMock.Object);

            _repositoryManagerMock.SetupGet(x => x.Category)
                .Returns(_categoryRepositoryMock.Object);

            _handler = new UpdateWorkTaskHandler(_repositoryManagerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateTask_When_Found()
        {
            var userId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var updateWorkTaskDto = new UpdateWorkTaskDto(
                Id: taskId,
                Title: "New Title",
                Description: "New Desc",
                DueDate: DateTime.Now,
                Priority: Priority.High,
                Status: WorkTaskStatus.InProgress,
                CategoryId: categoryId,
                TagIds: null);

            var command = new UpdateWorkTaskCommand(taskId, userId, updateWorkTaskDto);

            var existedTask = new WorkTask
            {
                Id = taskId,
                UserId = userId,
                Title = "For update tests",
                Tags = new List<Tag>()
            };

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, true))
                .ReturnsAsync(existedTask);

            _categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(categoryId, userId, false))
                .ReturnsAsync(new Category { Id = categoryId });

            await _handler.Handle(command, CancellationToken.None);

            existedTask.Title.Should().Be("New Title");
            existedTask.Description.Should().Be("New Desc");
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_UpdateTags_When_TagsIdsProvided()
        {
            var userId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid();
            var newTagId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var updateTaskDto = new UpdateWorkTaskDto(
                Id: taskId,
                Title: "Updated Title",
                Description: string.Empty,
                DueDate: DateTime.Now,
                Priority: Priority.Low,
                Status: WorkTaskStatus.Todo,
                CategoryId: categoryId,
                TagIds: new List<Guid>() { newTagId });

            var command = new UpdateWorkTaskCommand(taskId, userId, updateTaskDto);

            var existingEntity = new WorkTask
            {
                Id = taskId,
                UserId = userId,
                Tags = new List<Tag>()
            };

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, true))
                .ReturnsAsync(existingEntity);

            _categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(categoryId, userId, false))
                .ReturnsAsync(new Category { Id = categoryId });

            var tagsFromDb = new List<Tag> { new Tag { Id = newTagId, Name = "new tag" } };

            _tagRepositoryMock.Setup(x => x.GetTagsByIdsAsync(updateTaskDto.TagIds, userId, false))
                .ReturnsAsync(tagsFromDb);

            await _handler.Handle(command, CancellationToken.None);

            existingEntity.Tags.Should().HaveCount(1);
            existingEntity.Tags.First().Id.Should().Be(newTagId);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowBadRequest_When_IdsDoNotMatch()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            var updateTaskDto = new UpdateWorkTaskDto(
                Id: Guid.NewGuid(),
                Title: "Updated Title",
                Description: string.Empty,
                DueDate: DateTime.Now,
                Priority: Priority.Low,
                Status: WorkTaskStatus.Todo,
                CategoryId: Guid.NewGuid(),
                TagIds: new List<Guid>());

            var command = new UpdateWorkTaskCommand(taskId, userId, updateTaskDto);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<IdParametersBadRequestException>();
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFound_When_TaskDoesNotExist()
        {
            var userId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid();

            var updateWorkTaskDto = new UpdateWorkTaskDto(
              Id: taskId,
              Title: "New Title",
              Description: "New Desc",
              DueDate: DateTime.Now,
              Priority: Priority.High,
              Status: WorkTaskStatus.InProgress,
              CategoryId: Guid.NewGuid(),
              TagIds: null);

            var command = new UpdateWorkTaskCommand(taskId, userId, updateWorkTaskDto);

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, true))
                .ReturnsAsync((WorkTask?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<TaskNotFoundException>();

            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}