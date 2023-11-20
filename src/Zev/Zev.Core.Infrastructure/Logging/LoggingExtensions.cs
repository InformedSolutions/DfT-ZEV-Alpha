using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;

namespace Zev.Core.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        var serilogConfiguration = new LoggerConfiguration()
         .Enrich.FromLogContext() // Enriches log events with properties from the log context
         .Enrich.WithExceptionDetails() // Enriches with exception details
         .WriteTo.Console(
             outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId}] {Message:lj} {NewLine}{Exception} {Properties:j}")
         .ReadFrom.Configuration(configuration);

        services.AddSingleton<ILogger>(x => serilogConfiguration.CreateLogger());

        return services;
    }
}