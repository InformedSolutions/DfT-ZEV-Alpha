using FluentAssertions;
using NetArchTest.Rules;

namespace DfT.ZEV.Core.Architecture.Tests;

[TestFixture]
public class InfrastructureTests : BaseTest
{
    [Test]
    public void Repositories_ShouldBeInternalAndSealed()
    {
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .AreClasses()
            .And()
            .HaveNameEndingWith("Repository")
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }
}