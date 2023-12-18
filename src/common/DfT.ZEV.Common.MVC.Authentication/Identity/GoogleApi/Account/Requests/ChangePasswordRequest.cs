namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;

/// <summary>
/// Represents a request to change the password for a user account.
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Gets or sets the one-time password reset code.
    /// </summary>
    public string OobCode { get; set; }

    /// <summary>
    /// Gets or sets the new password for the user account.
    /// </summary>
    public string NewPassword { get; set; }

    /// <summary>
    /// Gets or sets the ID of the tenant associated with the user account.
    /// </summary>
    public string TenantId { get; set; }
}