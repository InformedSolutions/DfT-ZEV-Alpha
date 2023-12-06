namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public interface IGoogleIdentityApiClient
{
    Task<AuthorisationResponse> Authorise(string mail, string password, string tenantId);
    Task<RefreshTokenResponse> RefreshToken(string token);
    Task<PasswordResetTokenResponse> GetPasswordResetToken(PasswordResetTokenRequest passwordResetCodeRequest);
    Task ChangePasswordWithToken(PasswordChangeWithTokenRequest passwordChangeRequest);
}
