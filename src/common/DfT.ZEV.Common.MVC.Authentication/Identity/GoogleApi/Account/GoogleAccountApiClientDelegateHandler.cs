using System.Net.Http.Headers;
using DfT.ZEV.Common.Configuration;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account;

public class GoogleAccountApiClientDelegateHandler : DelegatingHandler
{
    private readonly IOptions<GoogleCloudConfiguration> _options;
    private readonly string[] _authorizedEndpoints = { "accounts:sendOobCode", "accounts:resetPassword" };
    
    public GoogleAccountApiClientDelegateHandler(IOptions<GoogleCloudConfiguration> options) => _options = options;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if(_authorizedEndpoints.Any(x => request.RequestUri != null && request.RequestUri.OriginalString.Contains(x)))
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