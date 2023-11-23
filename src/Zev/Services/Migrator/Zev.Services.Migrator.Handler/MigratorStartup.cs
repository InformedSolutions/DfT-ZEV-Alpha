using System.IO;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Logging;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Services.Migrator.Handler;

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
        
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(postgresSettings.ConnectionString);
        });

        services.AddSerilog(configuration);
        services.AddHttpContextAccessor();

    }
}