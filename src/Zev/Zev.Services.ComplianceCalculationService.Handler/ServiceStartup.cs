using System;
using System.Collections.Generic;
using System.IO;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Logging;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ComplianceCalculationService.Handler.Maps;
using Zev.Services.ComplianceCalculationService.Handler.Middleware;
using Zev.Services.ComplianceCalculationService.Handler.Processing;

namespace Zev.Services.ComplianceCalculationService.Handler;

public class ServiceStartup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var postgresSettings = services.ConfigurePostgresSettings(configuration);
        var bucketSettings = services.ConfigureBucketSettings(configuration);
        
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(postgresSettings.ConnectionString, conf =>
            {
                conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), new List<string> { "4060" });
            });
        });
        
        services.AddRepositories();
        
        services.AddSerilog(configuration);

        services.AddTransient<IProcessingService, ChunkProcessingService>();
        services.AddAutoMapper(typeof(VehicleMapper));

        services.AddHttpContextAccessor();
    }
    
    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalErrorHandler>();
    }
}