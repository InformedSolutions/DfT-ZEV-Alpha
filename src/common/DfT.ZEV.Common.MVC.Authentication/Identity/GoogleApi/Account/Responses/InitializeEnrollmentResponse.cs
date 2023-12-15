namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Responses;

public class StartMfaPhoneResponseInfo
{
    public string SessionInfo { get; set; }
}

public class InitializeEnrollmentResponse
{
    public StartMfaPhoneResponseInfo EnrollmentResponse { get; set; }

}