using System.Net;
using System.Net.Http.Headers;
using System.Text;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Authorize;
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
    private const string VerifyPasswordApiUrlTemplate = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={0}";
    private const string RefreshTokenApiUrlTemplate = "https://securetoken.googleapis.com/v1/token?key={0}";
    private const string GetOobCodeApiUrl = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode";
    private const string ResetPasswordApiUrl = "https://identitytoolkit.googleapis.com/v1/accounts:resetPassword";
    private const string MfaEnrollStartUrl = "https://identitytoolkit.googleapis.com/v2/accounts/mfaEnrollment:start";
    private static readonly string[] Scopes = { "https://www.googleapis.com/auth/cloud-platform", "https://www.googleapis.com/auth/firebase" };

    public GoogleIdentityApiClient(IOptions<GoogleCloudConfiguration> googleCloudConfiguration, HttpClient httpClient)
    {
        _googleCloudConfiguration = googleCloudConfiguration;
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<AuthorisationResponse> Authorise(string mail, string password, string tenantId)
    {
        var apiUrl = string.Format(VerifyPasswordApiUrlTemplate, _googleCloudConfiguration.Value.ApiKey);
        GoogleCredential.GetApplicationDefault().UnderlyingCredential.GetAccessTokenForRequestAsync();
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
        var apiUrl = string.Format(RefreshTokenApiUrlTemplate, _googleCloudConfiguration.Value.ApiKey);

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
        await ConfigureHttpClient();
        var requestJson = SerialiseToCamelCaseJson(passwordResetCodeRequest);
        var result = await _httpClient.PostAsync(GetOobCodeApiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<PasswordResetTokenResponse>(await result.Content.ReadAsStringAsync());
    }


    /// <inheritdoc/>
    public async Task ChangePasswordWithToken(PasswordChangeWithTokenRequest passwordChangeRequest)
    {
        await ConfigureHttpClient();
        var requestJson = SerialiseToCamelCaseJson(passwordChangeRequest);
        var result = await _httpClient.PostAsync(ResetPasswordApiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new ApplicationException($"Google API returned status code {result.StatusCode}");
        }
    }

    public async Task<StartEnrollmentResponse> EnrollMfa(StartEnrollmentRequest request)
    {
        await ConfigureHttpClient();
        var requestJson = SerialiseToCamelCaseJson(request);
        var result = await _httpClient.PostAsync(MfaEnrollStartUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));
        
        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new ApplicationException($"Google API returned status code {result.StatusCode}");
        }

        return JsonConvert.DeserializeObject<StartEnrollmentResponse>(await result.Content.ReadAsStringAsync());
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