namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi.Account.Responses;

/// <summary>
/// Represents the response object for looking up user information.
/// </summary>
public class LookupUserResponse
{
    /// <summary>
    /// Gets or sets the kind of the response.
    /// </summary>
    public string Kind { get; set; }

    /// <summary>
    /// Gets or sets the list of user data.
    /// </summary>
    public List<UserData> Users { get; set; }
}

/// <summary>
/// Represents the user data.
/// </summary>
public class UserData
{
    /// <summary>
    /// Gets or sets the local ID of the user.
    /// </summary>
    public string LocalId { get; set; }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the language of the user.
    /// </summary>
    public string Language { get; set; }

    /// <summary>
    /// Gets or sets the photo URL of the user.
    /// </summary>
    public string PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the time zone of the user.
    /// </summary>
    public string TimeZone { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// </summary>
    public string DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user's email is verified.
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the last login timestamp of the user.
    /// </summary>
    public string LastLoginAt { get; set; }

    /// <summary>
    /// Gets or sets the list of MFA (Multi-Factor Authentication) information for the user.
    /// </summary>
    public List<MfaInfo> MfaInfo { get; set; }
}

/// <summary>
/// Represents the MFA (Multi-Factor Authentication) information for a user.
/// </summary>
public class MfaInfo
{
    /// <summary>
    /// Gets or sets the MFA enrollment ID.
    /// </summary>
    public string MfaEnrollmentId { get; set; }

    /// <summary>
    /// Gets or sets the display name for the MFA.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the enrollment timestamp for the MFA.
    /// </summary>
    public string EnrolledAt { get; set; }
}