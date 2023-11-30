using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Services;
using Microsoft.Extensions.DependencyInjection;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using DfT.ZEV.Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure;

public static class InfrastructureDependencyInjection
{
    /// <summary>
    ///     This method adds the repositories and unit of workto the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IProcessRepository, ProcessRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
    }
    
    public static void AddDbContext(this IServiceCollection services, PostgresConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.ConnectionString,
                conf => { conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), new List<string> { "4060" }); });
        });
    }
}