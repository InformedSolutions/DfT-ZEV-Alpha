using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using DfT.ZEV.Core.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Tests.UnitOfWork;

[TestFixture]
public class UnitOfWorkTests
{
    private AppDbContext _dbContext = null!;
    private Infrastructure.UnitOfWork.UnitOfWork _unitOfWork = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _unitOfWork = new Infrastructure.UnitOfWork.UnitOfWork(_dbContext);
    }
    
    [Test]
    public async Task Repositories_ShouldNotBeNull()
    {
        // Assert
        _unitOfWork.Vehicles.Should().NotBeNull();
        _unitOfWork.Processes.Should().NotBeNull();
        _unitOfWork.Users.Should().NotBeNull();
        _unitOfWork.Manufacturers.Should().NotBeNull();
    }
   

    /* Transactions are not supported in in-memory databases.
    
    [Test]
    public async Task BeginTransactionAsync_ShouldBeginTransaction()
    {
        // Act
        var transactionId = await _unitOfWork.BeginTransactionAsync();

        // Assert
        transactionId.Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task CommitTransactionAsync_ShouldCommitTransaction()
    {
        // Arrange
        await _unitOfWork.BeginTransactionAsync();

        // Act
        await _unitOfWork.CommitTransactionAsync();

        // Assert
    }

    [Test]
    public async Task RollbackTransactionAsync_ShouldRollbackTransaction()
    {
        // Arrange
        await _unitOfWork.BeginTransactionAsync();

        // Act
        await _unitOfWork.RollbackTransactionAsync();

        // Assert
    }*/
}