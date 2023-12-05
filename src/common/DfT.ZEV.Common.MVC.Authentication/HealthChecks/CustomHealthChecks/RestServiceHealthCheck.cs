using System.Diagnostics;
using DfT.ZEV.Common.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Common.MVC.Authentication.HealthChecks.CustomHealthChecks;

public class RestServiceHealthCheck : IHealthCheck
{
    private readonly IOptions<ServicesConfiguration> _servicesConfiguration;

    public RestServiceHealthCheck(IOptions<ServicesConfiguration> servicesConfiguration)
    {
        _servicesConfiguration = servicesConfiguration;
    }

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