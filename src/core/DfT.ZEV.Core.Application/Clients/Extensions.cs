using DfT.ZEV.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace DfT.ZEV.Core.Application.Clients;

/// <summary>
/// Provides extension methods for configuring API service clients.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds the API service clients to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the clients to.</param>
    /// <param name="config">The application configuration.</param>
    /// <returns>The service collection with the added clients.</returns>
    public static IServiceCollection AddApiServiceClients(this IServiceCollection services, IConfiguration config)
    {
        var servicesConfiguration = config.GetServicesConfiguration();
        services.AddHttpClient<OrganisationApiClient>(options =>
        {
            options.BaseAddress = new Uri(servicesConfiguration.OrganisationApiBaseUrl);
            options.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddPolicyHandler(GetRetryPolicy())
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));


        return services;
    }

    /// <summary>
    /// Gets the retry policy for the HTTP client.
    /// </summary>
    /// <returns>The retry policy.</returns>
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

}