using System.IO;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using DfT.ZEV.Common.Logging;
using Google.Cloud.SecretManager.V1;
using DfT.ZEV.Common.Configuration;
using Notify.Interfaces;
using Notify.Client;
using DfT.ZEV.Common.Services;

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

        AddNotificationClient(services, configuration);

        services.AddScoped<INotificationsService, NotificationsService>();
    }

    private static void AddNotificationClient(IServiceCollection services, IConfiguration configuration)
    {
        var secretVersionName = new SecretVersionName(configuration.GetValue<string>("GoogleCloud:ProjectId"), configuration.GetValue<string>("NotifyApiKeySecretId"), "latest");
        var secretClient = SecretManagerServiceClient.Create();
        var secretAccessResponse = secretClient.AccessSecretVersion(secretVersionName);
        var notifyApiKey = secretAccessResponse.Payload.Data.ToStringUtf8();
        var notifyClient = new NotificationClient(notifyApiKey);
        services.AddSingleton<INotificationClient>(notifyClient);
    }
}