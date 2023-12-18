using System.Net.Http.Headers;
using DfT.ZEV.Common.Configuration;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Account;

/// <summary>
/// Represents a delegate handler for the Google Account API client.
/// </summary>
internal sealed class GoogleAccountApiClientDelegateHandler : DelegatingHandler
{
    private readonly IOptions<GoogleCloudConfiguration> _options;
    private readonly string[] _authorizedEndpoints = { "accounts:sendOobCode", "accounts:resetPassword","mfaEnrollment" };

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleAccountApiClientDelegateHandler"/> class.
    /// </summary>
    /// <param name="options">The Google Cloud configuration options.</param>
    public GoogleAccountApiClientDelegateHandler(IOptions<GoogleCloudConfiguration> options) => _options = options;

    /// <summary>
    /// Sends an HTTP request to the Google Account API with additional headers for authorized endpoints.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_authorizedEndpoints.Any(x => request.RequestUri != null && request.RequestUri.OriginalString.Contains(x)))
            await AddHeaders(request);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task AddHeaders(HttpRequestMessage req)
    {
        string[] scopes = { "https://www.googleapis.com/auth/cloud-platform", "https://www.googleapis.com/auth/firebase", "https://www.googleapis.com/auth/identitytoolkit" };

        var credential = GoogleCredential.GetApplicationDefault()
            .CreateScoped(scopes).CreateWithQuotaProject(_options.Value.ProjectId);

        var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

        var authHeader = new AuthenticationHeaderValue("Bearer", accessToken);
        req.Headers.Authorization = authHeader;
        req.Headers.Add("x-goog-user-project", _options.Value.ProjectId);
    }
}