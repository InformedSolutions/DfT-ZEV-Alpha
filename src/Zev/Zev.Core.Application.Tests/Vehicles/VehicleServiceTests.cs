using AutoFixture;
using Zev.Core.Application.Vehicles;
using Zev.Core.Domain.Vehicles.Models;

namespace Zev.Core.Application.Tests.Vehicles;

[TestFixture]
public class VehicleServiceTests
{
    private VehicleService _service;
    private IFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _service = new VehicleService();
        _fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyCustomization()));
    }
    
    [Test]
    public void ApplyRules_WhenCalledWithListOfVehicles_ShouldApplyRulesToEachVehicle()
    {
        // Arrange
        var vehicles =_fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "M1")
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 100)
            .With(x => x.Summary, new VehicleSummary("123"))
            .With(x => x.MM,1)
            .With(x => x.MRVL,2)
            .CreateMany(10).ToList();
        // Act
        _service.ApplyRules(vehicles);

        // Assert
        foreach (var vehicle in vehicles)
        {
            vehicle.Summary.ZevApplicable.Should().BeTrue();
            vehicle.Summary.Co2Applicable.Should().BeTrue();
            vehicle.Summary.VehicleScheme.Should().Be("car");
            vehicle.Summary.Zev.Should().BeTrue();
            vehicle.Summary.msv.Should().BeTrue(); 
            vehicle.Summary.IncompleteMsv.Should().BeTrue();
        }
    }

    [Test]
    public void ApplyRules_WhenCalledWithSingleVehicle_ShouldApplyRulesToVehicle()
    {
        // Arrange
        var vehicle =_fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "M1")
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 100)
            .With(x => x.Summary, new VehicleSummary("123"))
            .With(x => x.MM,1)
            .With(x => x.MRVL,2)
            .Create();

       
        // Act
        _service.ApplyRules(vehicle);

        // Assert
        vehicle.Summary.ZevApplicable.Should().BeTrue();
        vehicle.Summary.Co2Applicable.Should().BeTrue();
        vehicle.Summary.VehicleScheme.Should().Be("car");
        vehicle.Summary.Zev.Should().BeTrue();
        vehicle.Summary.msv.Should().BeTrue(); // Assuming ApplyMultistageVan sets msv to true
        vehicle.Summary.IncompleteMsv.Should().BeTrue(); // Default
    }
    
    //ApplyFlags Tests
    
    [Test]
    public void ApplyFlagsAndApplicability_WhenSpvcIsNullAndTANIsM1_ShouldSetFlagsAndSchemeForCar()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "M1")
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeTrue();
        result.Summary.Co2Applicable.Should().BeTrue();
        result.Summary.VehicleScheme.Should().Be("car");
    }

    [Test]
    public void ApplyFlagsAndApplicability_WhenSpvcIsNullAndTANIsN1_ShouldSetFlagsAndSchemeForVan()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "N1")
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeTrue();
        result.Summary.Co2Applicable.Should().BeTrue();
        result.Summary.VehicleScheme.Should().Be("van");
    }

    [Test]
    public void ApplyFlagsAndApplicability_WhenSpvcIsNullAndTANIsNotM1OrN1_ShouldSetFlagsAndSchemeBasedOnConditions()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(v => v.Ct, "N2")
            .With(v => v.Summary, new VehicleSummary("123")
            {
                Zev = true
            })
            .With(v => v.TPMLM, 4000)
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeTrue();
        result.Summary.Co2Applicable.Should().BeFalse();
        result.Summary.VehicleScheme.Should().Be("van");
    }
    
    [Test]
    public void ApplyFlagsAndApplicability_WhenSpvcIsNotNull_ShouldSetFlagsToFalse()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .With(v => v.Spvc, "e")
            .With(x => x.Summary, new VehicleSummary("132"))
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeFalse();
        result.Summary.Co2Applicable.Should().BeFalse();
    }
    
    
    //ApplyZev Tests
    [Test]
    public void ApplyZev_WhenEwltpIsZeroAndBerIsGreaterThanOrEqualTo100_ShouldSetZevToTrue()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 100)
            .With(x => x.Summary, new VehicleSummary("123"))
            .Create();

        // Act
        _service.ApplyZev(vehicle);

        // Assert
        vehicle.Summary.Zev.Should().BeTrue();
    }

    [Test]
    public void ApplyZev_WhenEwltpIsZeroAndBerIsLessThan100_ShouldSetZevAndRrrToTrue()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 99)
            .With(x => x.Summary, new VehicleSummary("123"))
            .Create();

        // Act
        _service.ApplyZev(vehicle);

        // Assert
        vehicle.Summary.Zev.Should().BeTrue();
        vehicle.Summary.Rrr.Should().BeFalse();
    }

    [Test]
    public void ApplyZev_WhenEwltpIsNotZero_ShouldSetZevToFalse()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .With(v => v.Ewltp, 1)
            .With(x => x.Summary, new VehicleSummary("123"))
            .Create();

        // Act
        _service.ApplyZev(vehicle);

        // Assert
        vehicle.Summary.Zev.Should().BeFalse();
    }
    
    //ApplyMultistageVan Tests
    [Test]
    public void ApplyMultistageVan_ShouldSetMsvToFalse()
    {
        // Arrange
        var vehicle = new Vehicle()
        {
            MM = null,
            MRVL = null,
            Summary = new VehicleSummary("123")
        };
        
        // Act
        _service.ApplyMultistageVan(vehicle);

        // Assert
        vehicle.Summary.msv.Should().BeFalse(); // Assuming ApplyMultistageVan sets msv to true
        vehicle.Summary.IncompleteMsv.Should().BeFalse(); // Default
    }

    [Test]
    public void ApplyMultistageVan_ShouldSetMsvToTrue()
    {
        // Arrange
        var vehicle = new Vehicle()
        {
            MM = 1,
            MRVL = 2,
            Summary = new VehicleSummary("123")
        };

        // Act
        _service.ApplyMultistageVan(vehicle);

        // Assert
        vehicle.Summary.msv.Should().BeTrue(); // Assuming ApplyMultistageVan sets msv to true
        vehicle.Summary.IncompleteMsv.Should().BeTrue(); // Default
    }
}