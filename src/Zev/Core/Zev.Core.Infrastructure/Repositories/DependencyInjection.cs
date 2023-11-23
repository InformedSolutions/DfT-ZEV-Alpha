using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Domain.Processes.Services;
using Zev.Core.Domain.Vehicles.Services;

namespace Zev.Core.Infrastructure.Repositories;

public static class DependencyInjection
{
    /// <summary>
    ///     This method adds the repositories to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IProcessRepository, ProcessRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}