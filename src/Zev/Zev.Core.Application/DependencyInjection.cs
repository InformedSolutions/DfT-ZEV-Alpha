using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Application.Vehicles;
using Zev.Core.Domain.Vehicles.Services;

namespace Zev.Core.Application;

public static class DependencyInjection
{
    /// <summary>
    /// This method adds the domain services to the service collection.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns></returns>
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IVehicleService, VehicleService>();

        return services;
    }
}