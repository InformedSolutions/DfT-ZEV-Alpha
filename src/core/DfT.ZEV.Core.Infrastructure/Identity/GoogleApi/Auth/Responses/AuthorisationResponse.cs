namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth.Responses;

/// <summary>
/// Represents the response received from the authorization endpoint of the Google API.
/// </summary>
public class AuthorisationResponse
{
    /// <summary>
    /// Gets or sets the kind of the authorization response.
    /// </summary>
    public string Kind { get; set; } = null!;

    /// <summary>
    /// Gets or sets the local ID associated with the user.
    /// </summary>
    public string LocalId { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID token received from the authorization server.
    /// </summary>
    public string IdToken { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the user is registered.
    /// </summary>
    public bool Registered { get; set; }

    /// <summary>
    /// Gets or sets the refresh token received from the authorization server.
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Gets or sets the expiration time of the ID token in seconds.
    /// </summary>
    public int ExpiresIn { get; set; }
}