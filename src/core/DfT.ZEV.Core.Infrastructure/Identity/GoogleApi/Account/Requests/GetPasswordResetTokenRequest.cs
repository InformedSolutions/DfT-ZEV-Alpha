namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Account.Requests;

/// <summary>
/// Represents a request to get a password reset token.
/// </summary>
public class GetPasswordResetTokenRequest
{
    /// <summary>
    /// Gets or sets the email address associated with the account.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public string TenantId { get; set; }
}

/// <summary>
/// Represents a request to get an out-of-band code.
/// </summary>
public class GetOobCodeRequest
{
    /// <summary>
    /// Gets the request type, which is set to "PASSWORD_RESET".
    /// </summary>
    public string RequestType { get; } = "PASSWORD_RESET";

    /// <summary>
    /// Gets a value indicating whether to return an out-of-band link.
    /// </summary>
    public bool ReturnOobLink { get; } = true;

    /// <summary>
    /// Gets or sets the email address associated with the account.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user IP address.
    /// </summary>
    public string UserIp { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public string TenantId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the target project ID.
    /// </summary>
    public string TargetProjectId { get; set; } = null!;
}