using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Authorize;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.PasswordChange;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.RefreshToken;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.ResetPassword;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public interface IGoogleIdentityApiClient
{
    /// <summary>
    /// Authorises the user with the provided email and password for a specific tenant.
    /// </summary>
    /// <param name="mail">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="tenantId">The tenant's identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the authorisation response.</returns>
    Task<AuthorisationResponse> Authorise(string mail, string password, string tenantId);

    /// <summary>
    /// Refreshes the token.
    /// </summary>
    /// <param name="token">The token to be refreshed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the refreshed token response.</returns>
    Task<RefreshTokenResponse> RefreshToken(string token);

    /// <summary>
    /// Gets the password reset token.
    /// </summary>
    /// <param name="passwordResetCodeRequest">The password reset code request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the password reset token response.</returns>
    Task<PasswordResetTokenResponse> GetPasswordResetToken(PasswordResetTokenRequest passwordResetCodeRequest);

    /// <summary>
    /// Changes the password with the provided token.
    /// </summary>
    /// <param name="passwordChangeRequest">The password change request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ChangePasswordWithToken(PasswordChangeWithTokenRequest passwordChangeRequest);
}
