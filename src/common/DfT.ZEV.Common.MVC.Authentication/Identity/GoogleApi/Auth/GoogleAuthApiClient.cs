using System.Net;
using System.Text;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth.Requests;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth;

public class GoogleAuthApiClient : GoogleApiClientBase
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<GoogleCloudConfiguration> _options;
    public GoogleAuthApiClient(HttpClient httpClient, IOptions<GoogleCloudConfiguration> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<AuthorisationResponse> Authorise(AuthorisationRequest req)
    {
        var url = $"https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={_options.Value.ApiKey}";

        var requestJson = SerialiseToCamelCaseJson(req);
        var result = await _httpClient.PostAsync(url, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<AuthorisationResponse>(await result.Content.ReadAsStringAsync());
    }
    
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