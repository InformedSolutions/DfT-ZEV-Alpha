using System;
using System.Collections.Generic;
using System.IO;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Application;
using DfT.ZEV.Core.Infrastructure;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DfT.ZEV.Core.Infrastructure.Persistence;
using DfT.ZEV.Services.ComplianceCalculation.Handler.Maps;
using DfT.ZEV.Services.ComplianceCalculation.Handler.Middleware;
using DfT.ZEV.Services.ComplianceCalculation.Handler.Processing;
using DfT.ZEV.Services.ComplianceCalculation.Handler.Validation;

namespace DfT.ZEV.Services.ComplianceCalculation.Handler;

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

       services.AddDbContext(postgresSettings);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBucketSettings(configuration);
        services.AddInfrastructureServices();
        services.AddApplication();
        services.AddApplication();
        //TO-DO: Add Serilog from commons
        //services.AddSerilog(configuration);
        services.AddLogging();
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