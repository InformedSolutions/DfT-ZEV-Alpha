using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Domain.Manufacturers.Services;
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
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddScoped<IPermissionRepository,PermissionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
    }
    
    public static void AddDbContext(this IServiceCollection services, PostgresConfiguration configuration)
    {
        services.AddDbContextPool<AppDbContext>(opt =>
        {
            // This causes errors while working in multithreaded processing, need to deep dive this topic
          //  opt.UseNpgsql(configuration.ConnectionString,
          //     conf => { conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), new List<string> { "4060" }); });
          opt.UseNpgsql(configuration.ConnectionString);
        });
    }
}