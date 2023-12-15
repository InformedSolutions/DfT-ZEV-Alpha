using System.Net;
using System.Text;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account;

/// <summary>
/// Represents a client for interacting with the Google Account API.
/// </summary>
internal sealed class GoogleAccountApiClient : GoogleApiClientBase, IGoogleAccountApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<GoogleCloudConfiguration> _options;

    /// <summary>
    /// Represents a client for interacting with the Google Account API.
    /// </summary>
    public GoogleAccountApiClient(HttpClient httpClient, IOptions<GoogleCloudConfiguration> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    /// <summary>
    /// Looks up a user using the Google Account API.
    /// </summary>
    /// <param name="request">The lookup user request.</param>
    /// <returns>The lookup user response.</returns>
    public async Task<LookupUserResponse> LookupUser(LookupUserRequest request)
    {
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={_options.Value.ApiKey}";

        var requestBody = SerialiseToCamelCaseJson(request);
        var result = await _httpClient.PostAsync(url, new StringContent(requestBody, Encoding.UTF8, "application/json"));

        return result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<LookupUserResponse>(await result.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// Retrieves a password reset token for the specified user.
    /// </summary>
    /// <param name="request">The request containing the user's email and tenant ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the password reset token.</returns>
    public async Task<GetPasswordResetTokenResponse> GetPasswordResetToken(GetPasswordResetTokenRequest request)
    {
        var url = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode";

        var apiRequest = new GetOobCodeRequest
        {
            UserIp = "127.0.0.1",
            TenantId = request.TenantId,
            TargetProjectId = _options.Value.ProjectId,
            Email = request.Email
        };

        var requestJson = SerialiseToCamelCaseJson(apiRequest);
        var result = await _httpClient.PostAsync(url, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        var response = result.StatusCode != HttpStatusCode.OK
            ? throw new ApplicationException($"Google API returned status code {result.StatusCode}")
            : JsonConvert.DeserializeObject<GetOobCodeResponse>(await result.Content.ReadAsStringAsync());

        return new GetPasswordResetTokenResponse
        {
            Code = response.PasswordResetToken
        };
    }

    /// <summary>
    /// Changes the password for a user using a token.
    /// </summary>
    /// <param name="passwordChangeRequest">The request object containing the necessary information for changing the password.</param>
    /// <returns>A <see cref="ChangePasswordResponse"/> object representing the result of the password change operation.</returns>
    public async Task<ChangePasswordResponse> ChangePasswordWithToken(ChangePasswordRequest passwordChangeRequest)
    {
        const string resetPasswordApiUrl = "https://identitytoolkit.googleapis.com/v1/accounts:resetPassword";

        var requestJson = SerialiseToCamelCaseJson(passwordChangeRequest);
        var result = await _httpClient.PostAsync(resetPasswordApiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new ApplicationException($"Google API returned status code {result.StatusCode}");
        }

        return new ChangePasswordResponse();
    }

    /// <summary>
    /// Enrolls a user in Multi-Factor Authentication (MFA) by making a request to the Google API.
    /// </summary>
    /// <param name="request">The request object containing the necessary information for MFA enrollment.</param>
    /// <returns>An instance of the <see cref="InitializeEnrollmentResponse"/> class representing the response from the Google API.</returns>
    public async Task<InitializeEnrollmentResponse> EnrollMfa(InitializeMFAEnrollmentRequest request)
    {
        const string mfaEnrollStartUrl = "https://identitytoolkit.googleapis.com/v2/accounts/mfaEnrollment:start";
        var requestJson = SerialiseToCamelCaseJson(request);
        var result = await _httpClient.PostAsync(mfaEnrollStartUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));
        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new ApplicationException($"Google API returned status code {result.StatusCode}");
        }

        return JsonConvert.DeserializeObject<InitializeEnrollmentResponse>(await result.Content.ReadAsStringAsync());
    }
}