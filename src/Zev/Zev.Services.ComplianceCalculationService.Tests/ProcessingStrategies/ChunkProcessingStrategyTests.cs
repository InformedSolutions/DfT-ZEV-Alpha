using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Persistence;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

namespace Zev.Services.ComplianceCalculationService.Tests.ProcessingStrategies;

[TestFixture]
public class ChunkProcessingStrategyTests
{
    private Mock<ILogger> _loggerMock;
    private Mock<AppDbContext> _contextMock;
    private Mock<IMapper> _mapperMock;
    private Mock<DbSet<Vehicle>> _vehicleSetMock;
    private ChunkProcessingStrategy _strategy;
    private IFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyCustomization()));
        _loggerMock = new Mock<ILogger>();
        _contextMock = new Mock<AppDbContext>();
        _mapperMock = new Mock<IMapper>();
        _vehicleSetMock = new Mock<DbSet<Vehicle>>();
        _contextMock.Setup(c => c.Vehicles).Returns(_vehicleSetMock.Object);
        _strategy = new ChunkProcessingStrategy(_contextMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task ProcessAsync_ShouldProcessRecords()
    {
        //This test will be rewritten
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(Helpers.GetTestVehiclesFromFile()));
        var vehicles = _fixture.CreateMany<Vehicle>().ToList();
        _mapperMock.Setup(m => m.Map<IEnumerable<Vehicle>>(It.IsAny<IEnumerable<RawVehicleDTO>>()))
            .Returns(vehicles);

        // Act
        var result = await _strategy.ProcessAsync(stream,10);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Count.Should().Be(50);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Exactly(5));
    }
}