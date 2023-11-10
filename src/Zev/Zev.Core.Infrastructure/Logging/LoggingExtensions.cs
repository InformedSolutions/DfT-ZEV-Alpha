using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Zev.Core.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        var serilogConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration);

        services.AddSingleton<ILogger>(x => serilogConfiguration.CreateLogger());

        return services;
    }
}