using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.ZEV.Common.Configuration;

/// <summary>
///     Provides extension methods for configuring settings in the application's configuration.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Extension method to configure and validate Postgres settings from IConfiguration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="configuration">The <see cref="IConfiguration" /> instance.</param>
    /// <returns>The <see cref="PostgresConfiguration" /> instance with data.</returns>
    public static PostgresConfiguration ConfigurePostgresSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = new PostgresConfiguration();
        configuration.Bind(PostgresConfiguration.SectionName, settings);

        //To-Do: Add Validation of configurations.
        services.AddOptions<PostgresConfiguration>()
            .BindConfiguration(PostgresConfiguration.SectionName);

        return settings;
    }

    /// <summary>
    ///     Configures the settings for a bucket.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the settings to.</param>
    /// <param name="configuration">The configuration that contains the bucket settings.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static BucketsConfiguration ConfigureBucketSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = new BucketsConfiguration();
        configuration.Bind(BucketsConfiguration.SectionName, settings);

        //To-Do: Add Validation of configurations.
        services.AddOptions<BucketsConfiguration>()
            .BindConfiguration(BucketsConfiguration.SectionName);

        return settings;
    }

    /// <summary>
    ///     Configures the settings for a GCP.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the settings to.</param>
    /// <param name="configuration">The configuration that contains the GCP settings.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static GoogleCloudConfiguration ConfigureGoogleCloudSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetGoogleCloudSettings();

        services.AddOptions<GoogleCloudConfiguration>()
            .BindConfiguration(GoogleCloudConfiguration.SectionName);

        return settings;
    }

    public static GoogleCloudConfiguration GetGoogleCloudSettings(this IConfiguration configuration)
    {
        var settings = new GoogleCloudConfiguration();
        configuration.Bind(GoogleCloudConfiguration.SectionName, settings);
        return settings;
    }

    public static ServicesConfiguration ConfigureServicesSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = new ServicesConfiguration();
        configuration.Bind(ServicesConfiguration.SectionName, settings);

        services.AddOptions<ServicesConfiguration>()
            .BindConfiguration(ServicesConfiguration.SectionName);

        return settings;
    }
    
    public static ServicesConfiguration GetServicesConfiguration(this IConfiguration configuration)
    {
        var settings = new ServicesConfiguration();
        configuration.Bind(ServicesConfiguration.SectionName, settings);
        return settings;
    }
}