using FluentAssertions;
using FluentValidation;
using Zev.Core.Domain.Vehicles.Validation.Validators;

namespace Zev.Core.Domain.Tests.Vehicles.Validation.Validators;

[TestFixture]
public class VinValidatorTests
{
    private VinValidator<string> _validator = null!;
    [SetUp]
    public void SetUp()
    {
        _validator = new VinValidator<string>();
    }

    [Test]
    [TestCase("1G4HR54K31U135335")]
    [TestCase("EHE1UNFH1NZU0ZL4L")]
    [TestCase("MGFHZB2J5DBHZUMVD")]
    [TestCase("YZK9XFDC706EV78AZ")]
    public async Task IsValid_WhenCalledWithValidVin_ShouldReturnTrue(string validVin)
    {
        // Act
        var result = await _validator.IsValidAsync(new ValidationContext<string>(validVin), validVin, default);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCase("123123123123123123123123")]
    [TestCase("12312312312312312")]
    [TestCase("YZK9XFDC706EV78AQ")]
    [TestCase("YYYY1231231ZZZZZZ")]
    [TestCase("PL3EM2W48UR96GFJB")]
    public async Task IsValid_WhenCalledWithInvalidVin_ShouldReturnFalse(string invalidVin)
    {
        // Act
        var result = await _validator.IsValidAsync(new ValidationContext<string>(invalidVin), invalidVin, default);

        // Assert
        result.Should().BeFalse();
    }
}