using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Zev.Core.Domain.Vehicles;

namespace Zev.Core.Architecture.Tests;

public class LayerTests : BaseTest
{
    [Test]
    public void Domain_Should_NotHaveAnyDependencies()
    {
        var res= Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny()
            .GetResult();
        
        res.IsSuccessful.Should().BeTrue();
    }
    
    [Test]
    public void Infrastructure_ShouldOnlyDependOnDomain()
    {
        var forbiddenRefs = GetAssembliesWithout(DomainAssembly).GetNames();
        
        var res = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOnAll(forbiddenRefs)
            .GetResult();
        
        res.IsSuccessful.Should().BeTrue();
    }
    
}