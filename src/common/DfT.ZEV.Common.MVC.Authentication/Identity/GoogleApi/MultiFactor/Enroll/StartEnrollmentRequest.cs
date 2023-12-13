namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.MultiFactor.Enroll;

public class PhoneEnrollmentInfo
{
    public string PhoneNumber { get; set; }
    public string RecaptchaToken { get; set; }
}

public class StartEnrollmentRequest
{
    public string IdToken { get; set; }
    public string TenantId { get; set; }
    public PhoneEnrollmentInfo enrollment_info { get; set; }
}