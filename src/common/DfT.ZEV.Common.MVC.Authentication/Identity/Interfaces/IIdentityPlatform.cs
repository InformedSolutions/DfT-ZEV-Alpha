using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Authorize;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.MultiFactor.Enroll;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.RefreshToken;
using DfT.ZEV.Common.MVC.Authentication.Identity.Requests;
using FirebaseAdmin.Auth;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.Interfaces;

public interface IIdentityPlatform
{
    /// <summary>
    /// Creates a new user with the specified user record arguments and tenant ID.
    /// </summary>
    /// <param name="userRecordArgs">The user record arguments.</param>
    /// <param name="tenantId">The tenant's identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user record.</returns>
    public ValueTask<UserRecord> CreateUser(UserRecordArgs userRecordArgs, string tenantId);

    /// <summary>
    /// Deletes a user asynchronously.
    /// </summary>
    /// <param name="userId">The identifier of the user to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task DeleteUserAsync(Guid userId, string tenantId);

    /// <summary>
    /// Sets the user claims for the specified user ID and tenant ID.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="claims">The claims to set.</param>
    /// <param name="tenantId">The tenant's identifier.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetUserClaimsAsync(Guid userId, IReadOnlyDictionary<string, object> claims, string tenantId);

    /// <summary>
    /// Gets the password reset token for the specified user ID and tenant ID.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="tenantId">The tenant's identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the password reset token.</returns>
    public Task<string> GetPasswordResetToken(Guid userId, string tenantId);

    /// <summary>
    /// Gets the password reset token for the specified email and tenant ID.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="tenantId">The tenant's identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the password reset token.</returns>
    public Task<string> GetPasswordResetToken(string email, string tenantId);

    /// <summary>
    /// Authenticates the user with the specified authentication request and tenant ID.
    /// </summary>
    /// <param name="authorizationRequest">The authentication request.</param>
    /// <param name="tenantId">The tenant's identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the authorisation response.</returns>
    public Task<AuthorisationResponse> AuthenticateUser(AuthenticationRequest authorizationRequest, string tenantId);

    /// <summary>
    /// Refreshes the user's token.
    /// </summary>
    /// <param name="refreshToken">The token to be refreshed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the refreshed token response.</returns>
    public Task<RefreshTokenResponse> RefreshUser(string refreshToken);

    /// <summary>
    /// Changes the user's password with the specified out of band code, new password, and tenant ID.
    /// </summary>
    /// <param name="oobCode">The out of band code.</param>
    /// <param name="newPassword">The new password.</param>
    /// <param name="tenantId">The tenant's identifier.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ChangePasswordAsync(string oobCode, string newPassword, string tenantId);

    public Task<StartEnrollmentResponse> EnrollMfa(StartEnrollmentRequest request);
}