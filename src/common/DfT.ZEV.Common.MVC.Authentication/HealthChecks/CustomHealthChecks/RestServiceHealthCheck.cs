using DfT.ZEV.Common.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Common.MVC.Authentication.HealthChecks.CustomHealthChecks;

/// <summary>
/// Represents a health check for a REST service.
/// </summary>
public class RestServiceHealthCheck : IHealthCheck
{
    private readonly IOptions<ServicesConfiguration> _servicesConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestServiceHealthCheck"/> class.
    /// </summary>
    /// <param name="servicesConfiguration">The services configuration.</param>
    public RestServiceHealthCheck(IOptions<ServicesConfiguration> servicesConfiguration)
    {
        _servicesConfiguration = servicesConfiguration;
    }


    /// <summary>
    /// Checks the health of the REST service.
    /// </summary>
    /// <param name="context">A context object associated with the current execution.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
    /// <returns>A <see cref="Task"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var apiBaseUrl = context.Registration.Name switch
        {
            "organization-api-service" => _servicesConfiguration.Value.OrganisationApiBaseUrl,
            _ => throw new ArgumentException("Unknown service name.")
        };

        if (string.IsNullOrEmpty(apiBaseUrl))
            return HealthCheckResult.Degraded("Service url not specified.");

        var url = apiBaseUrl + "/health";


        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync(request, cancellationToken);

        return !response.IsSuccessStatusCode ? HealthCheckResult.Unhealthy("Cannot connect to the service.") : HealthCheckResult.Healthy("Successfully connected to the service.");
    }
}