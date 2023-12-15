namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth.Responses;

/// <summary>
/// Represents the response received when refreshing an access token.
/// </summary>
public class RefreshTokenResponse
{
    /// <summary>
    /// Gets or sets the number of seconds until the access token expires.
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the type of the access token.
    /// </summary>
    public string TokenType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the refresh token used to obtain a new access token.
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID token associated with the authenticated user.
    /// </summary>
    public string IdToken { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user ID associated with the authenticated user.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the project ID associated with the authenticated user.
    /// </summary>
    public string ProjectId { get; set; } = null!;
}