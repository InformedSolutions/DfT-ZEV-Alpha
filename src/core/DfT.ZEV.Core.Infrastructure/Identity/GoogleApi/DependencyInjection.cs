using DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Account;
using DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi;

public static class DependencyInjection
{

    /// <summary>
    /// Adds Google API clients to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddGoogleApiClients(this IServiceCollection services)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

        services.AddHttpClient<IGoogleAuthApiClient, GoogleAuthApiClient>()
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy)
            .AddPolicyHandler(timeoutPolicy);

        services.AddTransient<GoogleAccountApiClientDelegateHandler>();
        services.AddHttpClient<IGoogleAccountApiClient, GoogleAccountApiClient>()
            .AddHttpMessageHandler<GoogleAccountApiClientDelegateHandler>()
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy)
            .AddPolicyHandler(timeoutPolicy); ;
    }
}
