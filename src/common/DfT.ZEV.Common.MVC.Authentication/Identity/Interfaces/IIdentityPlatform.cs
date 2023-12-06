using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;
using FirebaseAdmin.Auth;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public interface IIdentityPlatform
{
    public ValueTask<UserRecord> CreateUser(UserRecordArgs userRecordArgs, string tenantId);
    public Task SetUserClaimsAsync(Guid userId, IReadOnlyDictionary<string, object> claims, string tenantId);
    public Task<string> GetPasswordResetToken(Guid userId, string tenantId);
    public Task<AuthorisationResponse> AuthenticateUser(AuthenticationRequest authorizationRequest, string tenantId);
    public Task<RefreshTokenResponse> RefreshUser(string refreshToken);
    public Task ChangePasswordAsync(string oobCode, string newPassword, string tenantId);
}