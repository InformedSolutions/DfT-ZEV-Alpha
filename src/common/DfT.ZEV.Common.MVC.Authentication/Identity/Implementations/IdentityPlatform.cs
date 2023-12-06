using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

internal sealed class IdentityPlatform : IIdentityPlatform
{
    private readonly IOptions<GoogleCloudConfiguration> _googleCloudConfiguration;
    private readonly IGoogleIdentityApiClient _googleIdentityApiClient;
    private readonly IOptions<ServicesConfiguration> _servicesConfiguration;
    public IdentityPlatform(IOptions<GoogleCloudConfiguration> googleCloudConfiguration, IGoogleIdentityApiClient googleIdentityApiClient, IOptions<ServicesConfiguration> servicesConfiguration)
    {
        _googleCloudConfiguration = googleCloudConfiguration;
        _googleIdentityApiClient = googleIdentityApiClient;
        _servicesConfiguration = servicesConfiguration;

        if (FirebaseApp.DefaultInstance == null)
        {
            var options = new AppOptions
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                ProjectId = _googleCloudConfiguration.Value.ProjectId,
            };
            FirebaseApp.Create(options);
        }
    }

    public async ValueTask<UserRecord> CreateUser(UserRecordArgs userRecordArgs, string tenantId)
        => await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(tenantId)
            .CreateUserAsync(userRecordArgs);

    public async Task SetUserClaimsAsync(Guid userId, IReadOnlyDictionary<string, object> claims, string tenantId)
    {
        var user = await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(tenantId)
            .GetUserAsync(userId.ToString());

        await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(tenantId)
            .SetCustomUserClaimsAsync(user.Uid, claims);
    }

    public async Task<string> GetPasswordResetToken(Guid userId, string tenantId)
    {
        var user = await FirebaseAuth.DefaultInstance.TenantManager
             .AuthForTenant(tenantId)
             .GetUserAsync(userId.ToString());

        var rq = new PasswordResetTokenRequest
        {
            UserIp = "127.0.0.1",
            TenantId = tenantId,
            TargetProjectId = _googleCloudConfiguration.Value.ProjectId,
            Email = user.Email
        };

        var res = await _googleIdentityApiClient.GetPasswordResetToken(rq);
        return res.PasswordResetToken;
    }

    public async Task<AuthorisationResponse> AuthenticateUser(AuthenticationRequest authorizationRequest, string tenantId)
    {

        Console.WriteLine($"Authenticating user {authorizationRequest.Email} in tenant {tenantId}");

        return await _googleIdentityApiClient.Authorise(authorizationRequest.Email, authorizationRequest.Password,
                    tenantId);
    }

    public async Task<RefreshTokenResponse> RefreshUser(string refreshToken)
        => await _googleIdentityApiClient.RefreshToken(refreshToken);


    public async Task ChangePasswordAsync(string oobCode, string newPassword, string tenantId)
    {
        var request = new PasswordChangeWithTokenRequest
        {
            PasswordResetToken = oobCode,
            NewPassword = newPassword,
            TenantId = tenantId,
        };

        await _googleIdentityApiClient.ChangePasswordWithToken(request);
    }
}