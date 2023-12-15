using System.Net;
using System.Text;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth.Requests;
using DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth;

/// <summary>
/// Represents a client for interacting with the Google Authentication API.
/// </summary>
internal sealed class GoogleAuthApiClient : GoogleApiClientBase, IGoogleAuthApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<GoogleCloudConfiguration> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleAuthApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for making API requests.</param>
    /// <param name="options">The options for configuring the Google Cloud API.</param>
    public GoogleAuthApiClient(HttpClient httpClient, IOptions<GoogleCloudConfiguration> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    /// <summary>
    /// Authorizes a user with the Google Authentication API.
    /// </summary>
    /// <param name="req">The authorization request.</param>
    /// <returns>The authorization response.</returns>
    public async Task<AuthorisationResponse> Authorise(AuthorisationRequest req)
    {
        var url = $"https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={_options.Value.ApiKey}";

        var requestJson = SerialiseToCamelCaseJson(req);
        var result = await _httpClient.PostAsync(url, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<AuthorisationResponse>(await result.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// Refreshes an access token using the Google Authentication API.
    /// </summary>
    /// <param name="token">The refresh token.</param>
    /// <returns>The refresh token response.</returns>
    public async Task<RefreshTokenResponse> RefreshToken(string token)
    {
        var url = $"https://securetoken.googleapis.com/v1/token?key={_options.Value.ApiKey}";

        var request = new RefreshTokenRequest
        {
            RefreshToken = token
        };

        var requestJson = SerialiseToSnakeCaseJson(request);
        var result = await _httpClient.PostAsync(url, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<RefreshTokenResponse>(await result.Content.ReadAsStringAsync(), new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
    }
}