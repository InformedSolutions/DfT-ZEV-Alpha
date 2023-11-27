using AutoFixture;

namespace DfT.ZEV.Core.Infrastructure.Tests;
public class DateOnlyCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
    }
}