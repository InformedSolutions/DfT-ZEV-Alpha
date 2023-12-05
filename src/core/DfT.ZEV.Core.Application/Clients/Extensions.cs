using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Application.Clients.OrganisationApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace DfT.ZEV.Core.Application.Clients;

public static class Extensions
{
    public static IServiceCollection AddApiServiceClients(this IServiceCollection services, IConfiguration config)
    {
        var servicesConfiguration = config.GetServicesConfiguration();
        services.AddHttpClient<IOrganisationApiClient,OrganisationApiClient>(options =>
            {
                options.BaseAddress = new Uri(servicesConfiguration.OrganisationApiBaseUrl);
                options.DefaultRequestHeaders.Add("Accept", "application/json");
            })  
            .AddPolicyHandler(GetRetryPolicy())
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        
        return services;
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}