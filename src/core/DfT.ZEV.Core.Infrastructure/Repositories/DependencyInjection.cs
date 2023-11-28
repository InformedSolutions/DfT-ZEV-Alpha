using DfT.ZEV.Core.Domain.Accounts.Services;
using Microsoft.Extensions.DependencyInjection;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}