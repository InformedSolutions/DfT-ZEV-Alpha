using System.IO;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Logging;
using Zev.Core.Infrastructure.Persistence;
using Zev.Services.ComplianceCalculationService.Handler.Maps;
using Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

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
            opt.UseNpgsql(postgresSettings.ConnectionString);
        });
        
        services.AddSerilog(configuration);

        services.AddTransient<IProcessingStrategy, FixedChunkProcessingStrategy>();
        services.AddAutoMapper(typeof(VehicleMapper));

        services.AddHttpContextAccessor();
    }
}