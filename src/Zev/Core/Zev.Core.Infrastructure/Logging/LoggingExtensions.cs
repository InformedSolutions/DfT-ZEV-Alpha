using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.GoogleCloudLogging;

namespace Zev.Core.Infrastructure.Logging;

public static class LoggingExtensions
{
    /// <summary>
    ///     This methods adds Serilog to the application.
    /// </summary>
    public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        var googleConfig = new GoogleCloudLoggingSinkOptions
        {
            ProjectId = configuration.GetValue("GoogleCloud:ProjectId", "zev-dev")
        };

        var serilogConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext() // Enriches log events with properties from the log context
            .Enrich.WithExceptionDetails() // Enriches with exception details
            .WriteTo.Console(
                outputTemplate:
                "[{Service}][{Timestamp:HH:mm:ss} {Level:u3}][{CorrelationId}] {Message:lj} {NewLine}{Exception} ")
            .WriteTo.GoogleCloudLogging(googleConfig,
                outputTemplate:
                "[{Service}][{Timestamp:HH:mm:ss} {Level:u3}][{CorrelationId}] {Message:lj} {NewLine}{Exception} ")
            .ReadFrom.Configuration(configuration);


        services.AddSingleton<ILogger>(serilogConfiguration.CreateLogger());

        return services;
    }
}