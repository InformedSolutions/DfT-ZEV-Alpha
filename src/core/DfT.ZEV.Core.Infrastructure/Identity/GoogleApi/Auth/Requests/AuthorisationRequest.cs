namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Auth.Requests;

/// <summary>
/// Represents an authorization request for Google API.
/// </summary>
public class AuthorisationRequest
{
    /// <summary>
    /// Gets or sets the email associated with the user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password associated with the user.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets a value indicating whether to return a secure token.
    /// </summary>
    public bool ReturnSecureToken { get; } = true;

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public string TenantId { get; set; } = null!;
}