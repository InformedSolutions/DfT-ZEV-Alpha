using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Domain.Vehicles;

namespace Zev.Core.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}