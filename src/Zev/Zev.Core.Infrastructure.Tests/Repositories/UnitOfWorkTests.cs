using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;

namespace Zev.Core.Infrastructure.Tests.Repositories;

public class UnitOfWorkTests
{
    private Mock<TestableAppDbContext> _mockContext;
    private Mock<DbSet<Vehicle>> _mockDbSet;
    private Mock<IDbContextTransaction> _mockTransaction;
    private Mock<DatabaseFacade> _mockDatabase;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<TestableAppDbContext>();
        _mockDatabase = new Mock<DatabaseFacade>(_mockContext.Object);
        _mockTransaction = new Mock<IDbContextTransaction>();
        _mockContext.Setup(x => x.TestableDatabase).Returns(_mockDatabase.Object);
        _mockContext.Setup(x => x.BeginTransaction()).Returns(_mockTransaction.Object);
        _mockContext.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_mockTransaction.Object);   }

    [Test]
    public void SaveChanges_ShouldCallSaveChangesOnce()
    {
        // Arrange
        var unitOfWork = new UnitOfWork(_mockContext.Object);

        // Act
        unitOfWork.SaveChanges();

        // Assert
        _mockContext.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Test]
    public async Task SaveChangesAsync_ShouldCallSaveChangesAsyncOnce()
    {
        // Arrange
        var unitOfWork = new UnitOfWork(_mockContext.Object);

        // Act
        await unitOfWork.SaveChangesAsync();

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}