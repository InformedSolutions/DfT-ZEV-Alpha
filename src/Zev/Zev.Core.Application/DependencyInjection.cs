using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Application.Vehicles;
using Zev.Core.Domain.Vehicles.Services;

namespace Zev.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IVehicleService, VehicleService>();

        return services;
    }
}