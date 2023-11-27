using System.Reflection;
using DfT.ZEV.Core.Application.Vehicles;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using DfT.ZEV.Core.Infrastructure.Persistence;

namespace DfT.ZEV.Core.Architecture.Tests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Vehicle).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(VehicleService).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(AppDbContext).Assembly;

    private static readonly Assembly[] Assemblies = { DomainAssembly, ApplicationAssembly, InfrastructureAssembly };

    public static Assembly[] GetAssembliesWithout(params Assembly[] toExclude)
    {
        return Assemblies.Exclude(toExclude);
    }
}