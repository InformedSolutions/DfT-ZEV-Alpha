using System.Reflection;
using Zev.Core.Application.Vehicles;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Architecture.Tests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Vehicle).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(VehicleService).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(AppDbContext).Assembly;
    
    static readonly Assembly[] Assemblies = { DomainAssembly, ApplicationAssembly, InfrastructureAssembly };
    public static Assembly[] GetAssembliesWithout(params Assembly[] toExclude) => Assemblies.Exclude(toExclude);
}