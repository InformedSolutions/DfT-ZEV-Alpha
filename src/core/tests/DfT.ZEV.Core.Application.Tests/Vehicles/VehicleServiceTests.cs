using System.Reflection;
using AutoFixture;
using DfT.ZEV.Core.Application.Vehicles;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using MediatR;
using Moq;

namespace DfT.ZEV.Core.Application.Tests.Vehicles;

[TestFixture]
public class VehicleServiceTests
{
    private VehicleService _service = null!;
    private IFixture _fixture = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;

    [SetUp]
    public void SetUp()
    {
        // Initialize the service with mocked dependencies
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _service = new VehicleService(_mockUnitOfWork.Object);
        _fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyCustomization()));

        // Configure the mock to return a value for GetManufacturerNamesAsync
        _mockUnitOfWork
            .Setup(uow => uow.Manufacturers.GetManufacturerNamesAsync(default))
            .ReturnsAsync(new List<string> { "Manufacturer1", "Manufacturer2" });

        // Set the value of the private field using reflection
        var manufacturerNamesField = typeof(VehicleService).GetField("_manufacturerNames", BindingFlags.Instance | BindingFlags.NonPublic);
        manufacturerNamesField?.SetValue(_service, new List<string>());
    }


    //ApplyFlags Tests
    [Test]
    public void ApplyRules_WhenCalledWithListOfVehicles_AndIncompleteMsv_ShouldSetMsvAndIncompleteMsvToFalse()
    {
        // Arrange
        var vehicles = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "M1")
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 100)
            .With(x => x.Summary, new VehicleSummary("123"))
            .CreateMany(5).ToList();

        //Hack
        foreach (var vehicle in vehicles)
        {
            vehicle.MM = null;
            vehicle.MRVL = null;
        }

        // Act
        _service.ApplyRules(vehicles);

        // Assert
        foreach (var vehicle in vehicles)
        {
            vehicle.Summary.msv.Should().BeFalse();
            vehicle.Summary.IncompleteMsv.Should().BeFalse();
        }
    }


    [Test]
    public void ApplyRules_WhenCalledWithListOfVehicles_ShouldApplyRulesToEachVehicle()
    {
        // Arrange
        var vehicles = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "M1")
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 100)
            .With(x => x.Summary, new VehicleSummary("123"))
            .With(x => x.MM, 1)
            .With(x => x.MRVL, 2)
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
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "M1")
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 100)
            .With(x => x.Summary, new VehicleSummary("123"))
            .With(x => x.MM, 1)
            .With(x => x.MRVL, 2)
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
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNullAndTANIsN2_AndZevIsTrue_AndTPMLMIsLessThan4250_ShouldSetZevApplicableAndVehicleSchemeForVan()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(v => v.Ct, "N2")
            .With(x => x.Summary, new VehicleSummary("123") { Zev = true })
            .With(v => v.TPMLM, 4000)
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeTrue();
        result.Summary.VehicleScheme.Should().Be("van");
    }

    [Test]
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNullAndTANIsN2_AndZevIsTrue_AndTPMLMIsGreaterThanOrEqualTo4250_ShouldSetZevApplicableAndVehicleSchemeToFalse()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(v => v.Ct, "N2")
            .With(x => x.Summary, new VehicleSummary("123") { Zev = true })
            .With(v => v.TPMLM, 4250)
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeFalse();
        result.Summary.VehicleScheme.Should().BeNull();
    }


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
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNull_AndTANIsM1_ShouldSetZevApplicable_Co2Applicable_VehicleScheme_ForCar()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(v => v.Ct, "M1")
            .With(x => x.Summary, new VehicleSummary("123"))
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeTrue();
        result.Summary.Co2Applicable.Should().BeTrue();
        result.Summary.VehicleScheme.Should().Be("car");
    }

    [Test]
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNull_AndTANIsN1_ShouldSetZevApplicable_Co2Applicable_VehicleScheme_ForVan()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(v => v.Ct, "N1")
            .With(x => x.Summary, new VehicleSummary("123"))
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeTrue();
        result.Summary.Co2Applicable.Should().BeTrue();
        result.Summary.VehicleScheme.Should().Be("van");
    }

    [Test]
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNull_AndTANIsNotM1OrN1_AndCtIsN2_ZevIsTrue_AndTPMLMIsLessThan4250_ShouldSetZevApplicable_VehicleScheme_ForVan()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(v => v.Ct, "N2")
            .With(x => x.Summary, new VehicleSummary("123") { Zev = true })
            .With(v => v.TPMLM, 4000)
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeTrue();
        result.Summary.VehicleScheme.Should().Be("van");
    }

    [Test]
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNull_AndTANIsNotM1OrN1_AndCtIsN2_ZevIsTrue_AndTPMLMIsGreaterThanOrEqualTo4250_ShouldSetZevApplicableToFalse()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(v => v.Ct, "N2")
            .With(x => x.Summary, new VehicleSummary("123") { Zev = true })
            .With(v => v.TPMLM, 4250)
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.ZevApplicable.Should().BeFalse();
        result.Summary.VehicleScheme.Should().BeNull();
    }

    [Test]
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNull_AndTANIsNotM1OrN1_AndCtIsNotN2_ShouldSetCo2ApplicableToFalse_And_ZevApplicable_VehicleScheme_ToFalse()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(v => v.Ct, "Other")
            .With(x => x.Summary, new VehicleSummary("123"))
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.Co2Applicable.Should().BeFalse();
        result.Summary.ZevApplicable.Should().BeFalse();
        result.Summary.VehicleScheme.Should().BeNull();
    }

    [Test]
    public void
        ApplyFlagsAndApplicability_WhenSpvcIsNotNull_ShouldSetCo2Applicable_ZevApplicable_VehicleScheme_ToFalse()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .With(x => x.Spvc, "e")
            .With(x => x.Summary, new VehicleSummary("132"))
            .Create();

        // Act
        var result = _service.ApplyFlagsAndApplicability(vehicle);

        // Assert
        result.Summary.Co2Applicable.Should().BeFalse();
        result.Summary.ZevApplicable.Should().BeFalse();
        result.Summary.VehicleScheme.Should().BeNull();
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


    [Test]
    public void ApplyFlagsAndApplicability_WhenSpvcIsNotNull_ShouldSetZevAndCo2ApplicableToFalse()
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
    public void ApplyZev_WhenEwltpIsZero_AndBerIsEqualToMinRange_ShouldSetRrrToFalse()
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
        vehicle.Summary.Rrr.Should().BeFalse();
    }

    [Test]
    public void ApplyZev_WhenEwltpIsZero_AndBerIsLessThanMinRange_ShouldSetZevAndRrrToTrue()
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
    }


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


    [Test]
    public void ApplyZev_WhenEwltpIsZeroAndBerIsEqualToMinRange_ShouldSetRrrToFalse()
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
        vehicle.Summary.Rrr.Should().BeFalse();
    }

    [Test]
    public void ApplyZev_WhenEwltpIsNotZeroValue_ShouldSetZevToFalse()
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
        var vehicle = new Vehicle
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
        var vehicle = new Vehicle
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

    [Test]
    public void ApplyMultistageVan_WhenMMAndMRVLAreNull_ShouldSetMsvAndIncompleteMsvToFalse()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            MM = null,
            MRVL = null,
            Summary = new VehicleSummary("123")
        };

        // Act
        _service.ApplyMultistageVan(vehicle);

        // Assert
        vehicle.Summary.msv.Should().BeFalse();
        vehicle.Summary.IncompleteMsv.Should().BeFalse();
    }

    [Test]
    public void ApplyMultistageVan_WhenMMAndMRVLAreNotNull_ShouldSetMsvAndIncompleteMsvToTrue()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            MM = 1,
            MRVL = 2,
            Summary = new VehicleSummary("123")
        };

        // Act
        _service.ApplyMultistageVan(vehicle);

        // Assert
        vehicle.Summary.msv.Should().BeTrue();
        vehicle.Summary.IncompleteMsv.Should().BeTrue();
    }


    [Test]
    public void DetermineBonusCredits_WhenCalled_ShouldThrowNotImplementedException()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc) // Exclude auto-generation for this property
            .With(x => x.Summary, new VehicleSummary())
            .With(v => v.Ct, "M1")
            .With(v => v.Ewltp, 0)
            .With(v => v.Ber, 100)
            .With(x => x.Summary, new VehicleSummary("123"))
            .With(x => x.MM, 1)
            .With(x => x.MRVL, 2)
            .Create();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => _service.DetermineBonusCredits(vehicle));
    }

    [Test]
    public void DetermineBonusCredits_WhenCalledWithVehicle_ShouldThrowNotImplementedException()
    {
        // Arrange
        var vehicle = _fixture.Build<Vehicle>()
            .Without(x => x.Spvc)
            .With(x => x.Summary, new VehicleSummary())
            .Create();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => _service.DetermineBonusCredits(vehicle));
    }
}