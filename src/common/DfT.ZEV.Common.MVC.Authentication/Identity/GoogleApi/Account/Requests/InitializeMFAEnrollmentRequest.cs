namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;

public class PhoneEnrollmentInfo
{
    public string PhoneNumber { get; set; }
    public string RecaptchaToken { get; set; }
}

public class InitializeMFAEnrollmentRequest
{
    public string IdToken { get; set; }
    public string TenantId { get; set; }
    public PhoneEnrollmentInfo enrollment_info { get; set; }
}