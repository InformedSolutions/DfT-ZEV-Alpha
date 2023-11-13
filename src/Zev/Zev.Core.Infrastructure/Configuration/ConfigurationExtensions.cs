using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zev.Core.Infrastructure.Configuration;

/// <summary>
/// Provides extension methods for configuring settings in the application's configuration.
/// </summary>
public static class ConfigurationExtensions
{

    /// <summary>
    /// Extension method to configure and validate Postgres settings from IConfiguration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    /// <returns>The <see cref="PostgresConfiguration"/> instance with data.</returns>
    public static PostgresConfiguration ConfigurePostgresSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new PostgresConfiguration();
        configuration.Bind(PostgresConfiguration.SectionName, settings);
        
        //To-Do: Add Validation of configurations.
        services.AddOptions<PostgresConfiguration>()
            .BindConfiguration(PostgresConfiguration.SectionName);

        return settings;
    }
    
    public static BucketsConfiguration ConfigureBucketSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new BucketsConfiguration();
        configuration.Bind(BucketsConfiguration.SectionName, settings);
        
        //To-Do: Add Validation of configurations.
        services.AddOptions<BucketsConfiguration>()
            .BindConfiguration(BucketsConfiguration.SectionName);

        return settings;
    }
}