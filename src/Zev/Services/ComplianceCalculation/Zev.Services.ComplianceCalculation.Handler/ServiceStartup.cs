using System;
using System.Collections.Generic;
using System.IO;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Application;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Logging;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ComplianceCalculation.Handler.Maps;
using Zev.Services.ComplianceCalculation.Handler.Middleware;
using Zev.Services.ComplianceCalculation.Handler.Processing;
using Zev.Services.ComplianceCalculation.Handler.Validation;

namespace Zev.Services.ComplianceCalculation.Handler;

public class ServiceStartup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var configuration = BuildConfiguration();

        ConfigureDatabase(services, configuration);
        ConfigureServices(services, configuration);
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var postgresSettings = services.ConfigurePostgresSettings(configuration);

        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(postgresSettings.ConnectionString,
                conf => { conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), new List<string> { "4060" }); });
        });
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBucketSettings(configuration);
        services.AddRepositories();
        services.AddDomainServices();
        services.AddSerilog(configuration);
        services.AddTransient<GlobalErrorHandler>();
        services.AddTransient<IProcessingService, ChunkProcessingService>();
        services.AddTransient<CsvValidatorService>();
        services.AddAutoMapper(typeof(VehicleMapper));
        services.AddHttpContextAccessor();
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalErrorHandler>();
    }
}