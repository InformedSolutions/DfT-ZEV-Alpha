using Microsoft.Extensions.DependencyInjection;
using DfT.ZEV.Core.Application.Processes;
using DfT.ZEV.Core.Application.Vehicles;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;

namespace DfT.ZEV.Core.Application;

public static class ApplicationDependencyInjection
{
    /// <summary>
    ///     This method adds the domain services to the service collection.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns></returns>
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IVehicleService, VehicleService>();
        services.AddTransient<IProcessService,ProcessService>();
        return services;
    }
}