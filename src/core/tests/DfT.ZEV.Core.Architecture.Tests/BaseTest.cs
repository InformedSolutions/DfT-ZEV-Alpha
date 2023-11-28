using System.Reflection;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.Models;
using DfT.ZEV.Core.Application;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using DfT.ZEV.Core.Infrastructure.Persistence;

namespace DfT.ZEV.Core.Architecture.Tests;

public abstract class BaseTest
{
    //Core
    protected static readonly Assembly DomainAssembly = typeof(Vehicle).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ApplicationDependencyInjection).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(AppDbContext).Assembly;

    //Commons
    protected static readonly Assembly CommonAssembly = typeof(BucketsConfiguration).Assembly;
    protected static readonly Assembly CommonMvcAuthAssembly = typeof(BasicAuthUser).Assembly;


    protected static readonly Assembly[] Assemblies =
        { DomainAssembly, ApplicationAssembly, InfrastructureAssembly, CommonAssembly, CommonMvcAuthAssembly };

    public static Assembly[] GetAssembliesWithout(params Assembly[] toExclude)
    {
        return Assemblies.Exclude(toExclude);
    }
}