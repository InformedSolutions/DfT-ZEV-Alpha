using FluentAssertions;
using NetArchTest.Rules;

namespace DfT.ZEV.Core.Architecture.Tests;

public class LayerTests : BaseTest
{
    [Test]
    public void Domain_Should_NotHaveAnyDependencies()
    {
        var forbiddenRefs = GetAssembliesWithout(DomainAssembly).GetNames();
        var res = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace(DomainAssembly.GetName().Name)
            .ShouldNot()
            .HaveDependencyOnAny()
            .GetResult();

        res.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Infrastructure_ShouldOnlyDependOnDomainAndCommons()
    {
        var forbiddenRefs = GetAssembliesWithout(DomainAssembly, InfrastructureAssembly,CommonAssembly).GetNames();
        var res = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace(InfrastructureAssembly.GetName().Name)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenRefs)
            .GetResult();

        res.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Application_ShouldOnlyDependOnDomainAndInfrastructureAndCommons()
    {
        var forbiddenRefs =
            GetAssembliesWithout(ApplicationAssembly, DomainAssembly, InfrastructureAssembly, CommonAssembly, CommonMvcAuthAssembly)
                .GetNames();
        var res = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace(ApplicationAssembly.GetName().Name)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenRefs)
            .GetResult();

        res.IsSuccessful.Should().BeTrue();
    }
}