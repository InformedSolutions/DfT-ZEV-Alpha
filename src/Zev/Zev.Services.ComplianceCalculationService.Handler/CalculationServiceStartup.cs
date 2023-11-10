using System.IO;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Services.ComplianceCalculationService.Handler;

public class CalculationServiceStartup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var settings = services.ConfigurePostgresSettings(configuration);
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(settings.ConnectionString);
        });
    }
}