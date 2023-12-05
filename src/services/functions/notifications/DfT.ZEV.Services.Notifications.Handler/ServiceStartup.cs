using System.IO;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.Logging;
using DfT.ZEV.Common.Services;
using Notify.Interfaces;
using Notify.Client;

namespace DfT.ZEV.Services.Notifications.Handler;

public class ServiceStartup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var configuration = BuildConfiguration();
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

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithAssemblyCompilationModeLogging()
            .Enrich.WithBuildIdLogging()
            .Enrich.WithEnvironmentNameLogging()
            .CreateLogger();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

        var notifyClient = new NotificationClient(configuration.GetValue<string>("GovUkNotifyApiKey"));
        services.AddSingleton<INotificationClient>(notifyClient);

        services.AddScoped<INotificationsService, NotificationsService>();
    }
}