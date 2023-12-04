using System.Net;
using System.Text;
using DfT.ZEV.Common.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class GoogleIdentityApiClient : IGoogleIdentityApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<GoogleCloudConfiguration> _googleCloudConfiguration;
    private readonly string _verifyPasswordUrlTemplate = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={0}";

    public GoogleIdentityApiClient(IOptions<GoogleCloudConfiguration> googleCloudConfiguration, HttpClient httpClient)
    {
        _googleCloudConfiguration = googleCloudConfiguration;
        _httpClient = httpClient;
    }

    public async Task<AuthorizationResponse> Authorize(string mail, string password, string tenantId)
    {
        var apiUrl = string.Format(_verifyPasswordUrlTemplate, _googleCloudConfiguration.Value.ApiKey);
        
        var request = new AuthorizationRequest()
        {
            Email = mail,
            Password = password,
            TenantId = tenantId,
            ReturnSecureToken = true
        };
        
        var requestJson = JsonConvert.SerializeObject(request, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        
        var result = await _httpClient.PostAsync(apiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));
        
        if(result.StatusCode != HttpStatusCode.OK)
            throw new Exception($"Google API returned status code {result.StatusCode}");
        
        return JsonConvert.DeserializeObject<AuthorizationResponse>(await result.Content.ReadAsStringAsync());
    }
}