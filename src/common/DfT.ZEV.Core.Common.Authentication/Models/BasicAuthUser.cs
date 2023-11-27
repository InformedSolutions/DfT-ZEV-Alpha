namespace DfT.ZEV.Core.Common.Authentication.Models;

/// <summary>
/// A simple class for carrying credentials used for basic authentication.
/// </summary>
public class BasicAuthUser
{
    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password for authentication.
    /// </summary>
    public string Password { get; set; }
}
