using Application.Features.WorkTasks.Commands;
using Application.Features.WorkTasks.Handlers;
using AutoMapper;
using Domain.Contracts;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using FluentAssertions;
using Moq;
using Shared.DTO;
using TaskManager.Domain.Models;

namespace Tests.Features.WorkTasks
{
    public class UpdateWorkTaskHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly UpdateWorkTaskHandler _handler;

        public UpdateWorkTaskHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
            _mapperMock = new Mock<IMapper>();

            _repositoryManagerMock.Setup(x => x.Task)
                .Returns(_workTaskRepositoryMock.Object);

            _repositoryManagerMock.SetupGet(x => x.Tag)
                .Returns(_tagRepositoryMock.Object);

            _handler = new UpdateWorkTaskHandler(_repositoryManagerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateTask_When_Found()
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

            var existedTask = new WorkTask
            {
                Id = taskId,
                UserId = userId,
                Title = "For update tests",
                Tags = new List<Tag>()
            };

            _workTaskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, userId, true))
                .ReturnsAsync(existedTask);

            await _handler.Handle(command, CancellationToken.None);

            _mapperMock.Verify(x => x.Map(updateWorkTaskDto, existedTask), Times.Once);
            _repositoryManagerMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_UpdateTags_When_TagsIdsProvided()
        {
            var userId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid();
            var newTagId = Guid.NewGuid();

            var updateTaskDto = new UpdateWorkTaskDto(
                Id: taskId,
                Title: "Updated Title",
                Description: string.Empty,
                DueDate: DateTime.Now,
                Priority: Priority.Low,
                Status: WorkTaskStatus.Todo,
                CategoryId: Guid.NewGuid(),
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

            var tagsFromDb = new List<Tag> { new Tag { Id = newTagId, Name = "new tag" } };

            _tagRepositoryMock.Setup(x => x.GetTagsByIdsAsync(updateTaskDto.TagIds, false))
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
        public async Task Handle_Should_ThrowNotFound_When_TaskDoesMotExist()
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
