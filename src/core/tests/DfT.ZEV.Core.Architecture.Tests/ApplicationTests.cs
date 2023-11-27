using FluentAssertions;
using NetArchTest.Rules;

namespace DfT.ZEV.Core.Architecture.Tests;

[TestFixture]
public class ApplicationTests : BaseTest
{

    [Test]
    public void Services_ShouldBeInternalAndSealed()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .AreClasses()
            .And()
            .HaveNameEndingWith("Service")
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }
}