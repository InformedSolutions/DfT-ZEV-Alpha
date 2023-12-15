namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Account.Requests;

/// <summary>
/// Represents a request to lookup a user in the Google API.
/// </summary>
public class LookupUserRequest
{
    /// <summary>
    /// Gets or sets the ID token of the user.
    /// </summary>
    public string IdToken { get; set; }

    /// <summary>
    /// Gets or sets the email(s) of the user.
    /// </summary>
    public List<string> Email { get; set; }

    /// <summary>
    /// Gets or sets the tenant ID of the user.
    /// </summary>
    public string TenantId { get; set; }
}