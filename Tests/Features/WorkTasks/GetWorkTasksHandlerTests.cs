using Application.Features.WorkTasks.Handlers;
using Application.Features.WorkTasks.Quaries;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Domain.RequestFeatures;
using FluentAssertions;
using Moq;
using Shared.DTO;

namespace Tests.Features.WorkTasks
{
    public class GetWorkTasksHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly GetWorkTasksHandler _handler;

        public GetWorkTasksHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _workTaskRepositoryMock = new Mock<IWorkTaskRepository>();
            _mapperMock = new Mock<IMapper>();

            _repositoryManagerMock.Setup(x => x.Task)
                .Returns(_workTaskRepositoryMock.Object);

            _handler = new GetWorkTasksHandler(_repositoryManagerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnPagedList_WhenCalled()
        {
            var userId = Guid.NewGuid().ToString();
            var query = new GetWorkTasksQuery(userId, new WorkTaskParameters(), false);

            var pagedListFromDb = new PagedList<WorkTask>(new List<WorkTask>(), 0, 1, 10);

            var mappedDtos = new List<WorkTaskDto> { new WorkTaskDto(
                 Id: Guid.NewGuid(),
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
                 })};

            _workTaskRepositoryMock.Setup(x => x.GetAllTasksAsync(userId, It.IsAny<WorkTaskParameters>(), false))
                .ReturnsAsync(pagedListFromDb);

            _mapperMock.Setup(x => x.Map<IEnumerable<WorkTaskDto>>(pagedListFromDb))
                .Returns(mappedDtos);

            var (tasks, metaData) = await _handler.Handle(query, CancellationToken.None);

            tasks.Should().NotBeNull();
            tasks.Should().BeSameAs(mappedDtos);
            metaData.Should().NotBeNull();
        }
    }
}