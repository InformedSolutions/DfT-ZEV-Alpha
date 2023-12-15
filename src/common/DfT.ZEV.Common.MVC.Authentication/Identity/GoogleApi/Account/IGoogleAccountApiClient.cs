using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Responses;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account;

/// <summary>
/// Represents an interface for interacting with the Google Account API.
/// </summary>
public interface IGoogleAccountApiClient
{
    /// <summary>
    /// Looks up a user based on the provided request.
    /// </summary>
    /// <param name="request">The request containing the user lookup information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response with the user information.</returns>
    Task<LookupUserResponse> LookupUser(LookupUserRequest request);

    /// <summary>
    /// Retrieves a password reset token based on the provided request.
    /// </summary>
    /// <param name="request">The request containing the information required to generate the password reset token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response with the password reset token.</returns>
    Task<GetPasswordResetTokenResponse> GetPasswordResetToken(GetPasswordResetTokenRequest request);

    /// <summary>
    /// Changes the password using the provided token and new password.
    /// </summary>
    /// <param name="passwordChangeRequest">The request containing the password change information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response indicating the success or failure of the password change.</returns>
    Task<ChangePasswordResponse> ChangePasswordWithToken(ChangePasswordRequest passwordChangeRequest);

    /// <summary>
    /// Initializes the enrollment process for multi-factor authentication (MFA).
    /// </summary>
    /// <param name="request">The request containing the information required to initialize MFA enrollment.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response indicating the success or failure of the MFA enrollment initialization.</returns>
    Task<InitializeEnrollmentResponse> EnrollMfa(InitializeMFAEnrollmentRequest request);
}