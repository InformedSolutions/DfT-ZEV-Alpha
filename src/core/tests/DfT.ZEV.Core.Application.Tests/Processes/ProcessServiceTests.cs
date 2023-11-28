using System.Text.Json;
using AutoFixture;
using DfT.ZEV.Core.Application.Processes;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Processes.Values;
using DfT.ZEV.Core.Infrastructure.Repositories;
using DfT.ZEV.Core.Infrastructure.UnitOfWork;
using Moq;

namespace DfT.ZEV.Core.Application.Tests.Processes;

[TestFixture]
public class ProcessServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _processService = new ProcessService(_unitOfWorkMock.Object);
        _fixture = new Fixture();
    }

    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private ProcessService _processService = null!;
    private IFixture _fixture = null!;

    [Test]
    public async Task CreateProcessAsync_WhenCalled_ShouldCreateProcess()
    {
        // Arrange
        var id = Guid.NewGuid();
        var processType = _fixture.Create<ProcessTypeEnum>();
        _unitOfWorkMock
            .Setup(u => u.Processes.AddAsync(It.Is<Process>(p => p.Id == id && p.Type == processType),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _processService.CreateProcessAsync(id, processType);

        // Assert
        result.Id.Should().Be(id);
        result.Type.Should().Be(processType);
        result.State.Should().Be(ProcessStateEnum.Waiting);
        _unitOfWorkMock.Verify(
            u => u.Processes.AddAsync(It.Is<Process>(p => p.Id == id && p.Type == processType),
                It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task StartProcessAsync_WhenCalled_ShouldSetMetadata()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();

        var id = process.Id;
        var metadata = new { Key = "TestKey", Value = "TestValue" };
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(process);

        // Act
        var result = await _processService.StartProcessAsync(id, metadata);

        // Assert
        metadata.Should().BeEquivalentTo(result.Metadata.Deserialize(metadata.GetType()));
        _unitOfWorkMock.Verify(u => u.Processes.Update(process), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task FinishProcessAsync_WhenCalled_ShouldSetResult()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();

        var id = process.Id;
        var resultData = new { Key = "TestKey", Value = "TestValue" };
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(process);

        // Act
        var result = await _processService.FinishProcessAsync(id, resultData);

        // Assert
        resultData.Should().BeEquivalentTo(result.Result.Deserialize(resultData.GetType()));
        _unitOfWorkMock.Verify(u => u.Processes.Update(process), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task StartProcessAsync_ShouldStartProcess()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();

        var id = process.Id;
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(process);

        // Act
        var result = await _processService.StartProcessAsync(id);

        // Assert
        result.Should().BeEquivalentTo(process);
        result.State.Should().Be(ProcessStateEnum.Running);
        _unitOfWorkMock.Verify(u => u.Processes.Update(process), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task FinishProcessAsync_ShouldFinishProcess()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();

        var id = process.Id;
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(process);

        // Act
        var result = await _processService.FinishProcessAsync(id);

        // Assert
        result.Should().BeEquivalentTo(process);
        result.State.Should().Be(ProcessStateEnum.Finished);
        _unitOfWorkMock.Verify(u => u.Processes.Update(process), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task FailProcessAsync_ShouldFailProcess()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();
        var id = process.Id;
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(process);

        // Act
        var result = await _processService.FailProcessAsync(id);

        // Assert
        result.Should().BeEquivalentTo(process);
        result.State.Should().Be(ProcessStateEnum.Failed);
        _unitOfWorkMock.Verify(u => u.Processes.Update(process), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task StartProcessAsync_WhenProcessNotFound_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Process)null);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _processService.StartProcessAsync(id));
    }

    [Test]
    public async Task FinishProcessAsync_WhenProcessNotFound_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Process)null);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _processService.FinishProcessAsync(id));
    }

    [Test]
    public async Task FailProcessAsync_WhenProcessNotFound_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Processes.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Process)null);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _processService.FailProcessAsync(id));
    }
}