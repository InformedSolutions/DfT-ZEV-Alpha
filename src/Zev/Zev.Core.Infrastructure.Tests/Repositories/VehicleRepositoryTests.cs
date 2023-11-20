using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;

namespace Zev.Core.Infrastructure.Tests.Repositories;

public class VehicleRepositoryTests
{
    private IFixture _fixture;
    private Mock<AppDbContext> _mockContext;
    private Mock<DbSet<Vehicle>> _mockDbSet;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyCustomization()));
        _mockContext = new Mock<AppDbContext>();
        _mockDbSet = new Mock<DbSet<Vehicle>>();
        _mockContext.Setup(x => x.Set<Vehicle>()).Returns(_mockDbSet.Object);
    }
    
    [Test]
    public async Task Save_ShouldCallSaveChangesAsyncOnce()
    {
        // Arrange
        //var repository = new VehicleRepository(_mockContext.Object);

        // Act
        //await repository.Save();

        // Assert
        //_mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Dispose_ShouldCallDisposeOnce()
    {
        // Arrange
       // var repository = new VehicleRepository(_mockContext.Object);

        // Act
       // repository.Dispose();

        // Assert
       // _mockContext.Verify(x => x.Dispose(), Times.Once);
    }
}