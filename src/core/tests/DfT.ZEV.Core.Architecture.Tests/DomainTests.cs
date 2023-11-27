using NetArchTest.Rules;

namespace DfT.ZEV.Core.Architecture.Tests;

[TestFixture]
public class DomainTests : BaseTest
{
    [Test]
    public void DomainClasses_ShouldBeSealed()
    {
        
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .AreClasses()
            .Should()
            .BeSealed()
            .GetResult();
    }
}