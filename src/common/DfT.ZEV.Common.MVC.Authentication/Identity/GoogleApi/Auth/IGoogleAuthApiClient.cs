using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth.Requests;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth.Responses;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth;


/// <summary>
/// Represents an interface for the Google Auth API client.
/// </summary>
public interface IGoogleAuthApiClient
{
    /// <summary>
    /// Authorizes the user with the specified authorization request.
    /// </summary>
    /// <param name="req">The authorization request.</param>
    /// <returns>The authorization response.</returns>
    Task<AuthorisationResponse> Authorise(AuthorisationRequest req);

    /// <summary>
    /// Refreshes the access token using the specified refresh token.
    /// </summary>
    /// <param name="token">The refresh token.</param>
    /// <returns>The refresh token response.</returns>
    Task<RefreshTokenResponse> RefreshToken(string token);
}