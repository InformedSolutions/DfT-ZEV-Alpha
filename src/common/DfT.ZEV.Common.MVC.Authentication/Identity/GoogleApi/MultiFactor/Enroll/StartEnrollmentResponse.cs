namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.MultiFactor.Enroll;

public class StartMfaPhoneResponseInfo
{
    public string SessionInfo { get; set; }
}

public class StartEnrollmentResponse
{
    public StartMfaPhoneResponseInfo enrollment_response { get; set; }
}