using DfT.ZEV.Core.Application.Accounts.Commands.CreateManufacturer;
using DfT.ZEV.Core.Application.Common;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace DfT.ZEV.Core.Application.Tests.Accounts;

[TestFixture]
public class CreateManufacturerCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<ILogger<CreateManufacturerCommandHandler>> _loggerMock = null!;
    private CreateManufacturerCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateManufacturerCommandHandler>>();
        _handler = new CreateManufacturerCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldInsertManufacturer_WhenManufacturerDoesNotExist()
    {
        // Arrange
        var command = new CreateManufacturerCommand
        {
            Name = "Test Manufacturer",
            Co2Target = 100,
            DerogationStatus = 's'
        };
        _unitOfWorkMock.Setup(u => u.Manufacturers.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Manufacturer)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.Manufacturers.InsertAsync(It.Is<Manufacturer>(m => m.Name == command.Name), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Handle_ShouldThrowException_WhenManufacturerAlreadyExists()
    {
        // Arrange
        var command = new CreateManufacturerCommand
        {
            Name = "Test Manufacturer",
            Co2Target = 100,
            DerogationStatus = 's'
        };
        _unitOfWorkMock.Setup(u => u.Manufacturers.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Manufacturer(command.Name));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().ThrowExactlyAsync<HandlerException>();
    }
}