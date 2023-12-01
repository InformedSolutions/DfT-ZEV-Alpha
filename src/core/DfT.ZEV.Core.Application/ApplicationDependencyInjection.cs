using DfT.ZEV.Core.Application.Accounts.Services;
using Microsoft.Extensions.DependencyInjection;
using DfT.ZEV.Core.Application.Processes;
using DfT.ZEV.Core.Application.Vehicles;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;
using MediatR;

namespace DfT.ZEV.Core.Application;

public static class ApplicationDependencyInjection
{
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDomainServices();
        services.AddAutoMapper(typeof(ApplicationDependencyInjection).Assembly);
        services.AddMediatR(typeof(ApplicationDependencyInjection).Assembly);
        return services;
    }
    
    /// <summary>
    ///     This method adds the domain services to the service collection.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns></returns>
    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IVehicleService, VehicleService>();
        services.AddTransient<IProcessService,ProcessService>();
        services.AddTransient<IUsersService,UsersService>();
        return services;
    }
    
}