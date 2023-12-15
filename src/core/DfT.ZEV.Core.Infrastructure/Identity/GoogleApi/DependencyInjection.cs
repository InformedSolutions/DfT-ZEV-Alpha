using DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Account;
using DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi;

public static class DependencyInjection
{

    /// <summary>
    /// Adds Google API clients to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddGoogleApiClients(this IServiceCollection services)
    {
        services.AddHttpClient<IGoogleAuthApiClient, GoogleAuthApiClient>();

        services.AddTransient<GoogleAccountApiClientDelegateHandler>();
        services.AddHttpClient<IGoogleAccountApiClient, GoogleAccountApiClient>()
            .AddHttpMessageHandler<GoogleAccountApiClientDelegateHandler>();
    }
}