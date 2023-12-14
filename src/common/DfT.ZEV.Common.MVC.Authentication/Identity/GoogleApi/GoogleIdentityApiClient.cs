using System.Net;
using System.Net.Http.Headers;
using System.Text;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Authorize;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Lookup;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.MultiFactor.Enroll;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.PasswordChange;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.RefreshToken;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.ResetPassword;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class GoogleIdentityApiClient : IGoogleIdentityApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<GoogleCloudConfiguration> _googleCloudConfiguration;
    private static readonly string[] Scopes = { "https://www.googleapis.com/auth/cloud-platform", "https://www.googleapis.com/auth/firebase" };

    public GoogleIdentityApiClient(IOptions<GoogleCloudConfiguration> googleCloudConfiguration, HttpClient httpClient)
    {
        _googleCloudConfiguration = googleCloudConfiguration;
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<AuthorisationResponse> Authorise(string mail, string password, string tenantId)
    {
        const string verifyPasswordApiUrlTemplate = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={0}";
        var apiUrl = string.Format(verifyPasswordApiUrlTemplate, _googleCloudConfiguration.Value.ApiKey);
        //GoogleCredential.GetApplicationDefault().UnderlyingCredential.GetAccessTokenForRequestAsync();
        var request = new AuthorisationRequest
        {
            Email = mail,
            Password = password,
            TenantId = tenantId,
            ReturnSecureToken = true
        };

        var requestJson = SerialiseToCamelCaseJson(request);
        var result = await _httpClient.PostAsync(apiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<AuthorisationResponse>(await result.Content.ReadAsStringAsync());
    }

    /// <inheritdoc/>
    public async Task<RefreshTokenResponse> RefreshToken(string token)
    {
        const string refreshTokenApiUrlTemplate = "https://securetoken.googleapis.com/v1/token?key={0}";

        var apiUrl = string.Format(refreshTokenApiUrlTemplate, _googleCloudConfiguration.Value.ApiKey);

        var request = new RefreshTokenRequest
        {
            RefreshToken = token
        };

        var requestJson = SerialiseToSnakeCaseJson(request);
        var result = await _httpClient.PostAsync(apiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

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

    /// <inheritdoc/>
    public async Task<PasswordResetTokenResponse> GetPasswordResetToken(PasswordResetTokenRequest passwordResetCodeRequest)
    {
        const string getOobCodeApiUrl = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode";

        await ConfigureHttpClient();
        var requestJson = SerialiseToCamelCaseJson(passwordResetCodeRequest);
        var result = await _httpClient.PostAsync(getOobCodeApiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<PasswordResetTokenResponse>(await result.Content.ReadAsStringAsync());
    }


    /// <inheritdoc/>
    public async Task ChangePasswordWithToken(PasswordChangeWithTokenRequest passwordChangeRequest)
    {
        const string resetPasswordApiUrl = "https://identitytoolkit.googleapis.com/v1/accounts:resetPassword";

        await ConfigureHttpClient();
        var requestJson = SerialiseToCamelCaseJson(passwordChangeRequest);
        var result = await _httpClient.PostAsync(resetPasswordApiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new ApplicationException($"Google API returned status code {result.StatusCode}");
        }
    }

    public async Task<StartEnrollmentResponse> EnrollMfa(StartEnrollmentRequest request)
    {
        const string mfaEnrollStartUrl = "https://identitytoolkit.googleapis.com/v2/accounts/mfaEnrollment:start";
        await ConfigureHttpClient();
        var requestJson = SerialiseToCamelCaseJson(request);
        var result = await _httpClient.PostAsync(mfaEnrollStartUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new ApplicationException($"Google API returned status code {result.StatusCode}");
        }

        return JsonConvert.DeserializeObject<StartEnrollmentResponse>(await result.Content.ReadAsStringAsync());
    }

    public async Task<LookupUserResponse> LookupUser(string idToken, string userEmail, string tenantId)
    {
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={_googleCloudConfiguration.Value.ApiKey}";

        var emails = new[] { userEmail };

        var requestBody = $"{{\"idToken\": \"{idToken}\", \"email\": {GetJsonArray(emails)}, \"tenantId\": \"{tenantId}\"}}";


        await ConfigureHttpClient();
        var result = await _httpClient.PostAsync(url, new StringContent(requestBody, Encoding.UTF8, "application/json"));
        
        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<LookupUserResponse>(await result.Content.ReadAsStringAsync());
    }

    private static string GetJsonArray(string[] values)
    {
        // Convert array of strings to a JSON array string
        var jsonArray = new StringBuilder("[");
        foreach (var value in values)
        {
            jsonArray.Append($"\"{value}\",");
        }
        jsonArray.Length--; // Remove the trailing comma
        jsonArray.Append("]");
        return jsonArray.ToString();
    }

    /// <summary>
    /// Private helper to serialise to camel case JSON.
    /// </summary>
    /// <param name="input">Object to be serialised.</param>
    /// <returns>Snake-case key formatted JSON object.</returns> 
    private static string SerialiseToCamelCaseJson(object input)
    {
        return JsonConvert.SerializeObject(input, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }

    /// <summary>
    /// Private helper to serialise to snake-case JSON.
    /// </summary>
    /// <param name="input">Object to be serialised.</param>
    /// <returns>Camel-case key formatted JSON object.</returns> 
    private static string SerialiseToSnakeCaseJson(object input)
    {
        return JsonConvert.SerializeObject(input, new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        });
    }

    /// <summary>
    /// Helper to configure the HTTP client with an access token and project ID for quota field requirements.
    /// </summary>
    private async Task ConfigureHttpClient()
    {
        var credential = GoogleCredential.GetApplicationDefault()
            .CreateScoped(Scopes).CreateWithQuotaProject(_googleCloudConfiguration.Value.ProjectId);

        var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        _httpClient.DefaultRequestHeaders.Add("x-goog-user-project", _googleCloudConfiguration.Value.ProjectId);
    }
}