namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;

/// <summary>
/// Represents the information required to enroll a phone for multi-factor authentication.
/// </summary>
public class PhoneEnrollmentInfo
{
    /// <summary>
    /// Gets or sets the phone number to be enrolled.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the reCAPTCHA token for verification.
    /// </summary>
    public string RecaptchaToken { get; set; }
}

/// <summary>
/// Represents a request to initialize multi-factor authentication enrollment.
/// </summary>
public class InitializeMFAEnrollmentRequest
{
    /// <summary>
    /// Gets or sets the ID token for authentication.
    /// </summary>
    public string IdToken { get; set; }

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public string TenantId { get; set; }

    /// <summary>
    /// Gets or sets the phone enrollment information.
    /// </summary>
    public PhoneEnrollmentInfo enrollment_info { get; set; }
}