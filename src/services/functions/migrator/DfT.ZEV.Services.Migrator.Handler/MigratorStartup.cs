using System.IO;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.ZEV.Services.Migrator.Handler;

public class MigratorStartup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var postgresSettings = services.ConfigurePostgresSettings(configuration);

        services.AddDbContext<AppDbContext>(opt => { opt.UseNpgsql(postgresSettings.ConnectionString); });
        
        //TO-DO: Add Serilog from commons
        //services.AddSerilog(configuration);
        services.AddLogging();
        services.AddHttpContextAccessor();
    }
}