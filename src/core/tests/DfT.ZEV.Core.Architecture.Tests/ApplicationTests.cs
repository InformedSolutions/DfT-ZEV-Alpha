using FluentAssertions;
using MediatR;
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
    
    [Test]
    public void MediatorHandler_ShouldHaveProperName()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Test]
    public void MediatorRequest_ShouldHaveProperName()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .HaveNameEndingWith("Query")
            .Or()
            .HaveNameEndingWith("Command")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}