namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth.Requests;

/// <summary>
/// Represents a request to refresh an access token using a refresh token.
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Gets the grant type for the token request, which is always "refresh_token".
    /// </summary>
    public string GrantType { get; } = "refresh_token";

    /// <summary>
    /// Gets or sets the refresh token used to obtain a new access token.
    /// </summary>
    public string RefreshToken { get; set; } = null!;
}