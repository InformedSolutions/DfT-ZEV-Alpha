using System.Web;

namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Account.Responses;

/// <summary>
/// Represents the response for getting a password reset token.
/// </summary>
public class GetPasswordResetTokenResponse
{
    /// <summary>
    /// Gets or sets the password reset token.
    /// </summary>
    public string Code { get; set; }
}

/// <summary>
/// Represents the response for getting an out-of-band (oob) code.
/// </summary>
public class GetOobCodeResponse
{
    /// <summary>
    /// Gets the password reset token extracted from the oob link.
    /// </summary>
    public string PasswordResetToken => HttpUtility.ParseQueryString(OobLink)["oobCode"] ?? throw new InvalidOperationException("oobCode not found in OobLink");

    /// <summary>
    /// Gets or sets the oob link.
    /// </summary>
    public string OobLink { get; set; } = null!;
}