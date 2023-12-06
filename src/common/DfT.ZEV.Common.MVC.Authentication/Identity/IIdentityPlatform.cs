using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;
using FirebaseAdmin.Auth;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public interface IIdentityPlatform
{
    public ValueTask<UserRecord> CreateUser(UserRecordArgs userRecordArgs);
    public Task SetUserClaimsAsync(Guid userId, IReadOnlyDictionary<string, object> claims);
    public Task<string> GetPasswordResetLink(Guid userId);
    public Task<AuthorizationResponse> AuthenticateUser(AuthenticationRequest authorizationRequest);
    public Task<RefreshTokenResponse> RefreshUser(string refreshToken);
    public Task<PasswordChangeResponse> ChangePasswordAsync(string oobCode, string newPassword);
}