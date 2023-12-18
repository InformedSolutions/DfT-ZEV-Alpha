namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Responses;

/// <summary>
/// Represents the response information for starting MFA phone enrollment.
/// </summary>
public class StartMfaPhoneResponseInfo
{
    /// <summary>
    /// Gets or sets the session information.
    /// </summary>
    public string SessionInfo { get; set; }
}

/// <summary>
/// Represents the response for initializing enrollment.
/// </summary>
public class InitializeEnrollmentResponse
{
    /// <summary>
    /// Gets or sets the enrollment response information.
    /// </summary>
    public StartMfaPhoneResponseInfo EnrollmentResponse { get; set; }
}