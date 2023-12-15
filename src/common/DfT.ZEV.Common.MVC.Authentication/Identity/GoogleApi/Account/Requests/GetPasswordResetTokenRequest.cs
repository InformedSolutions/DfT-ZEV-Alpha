namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;

public class GetPasswordResetTokenRequest
{
    public string Email { get; set; }
    public string TenantId { get; set; }
}

public class GetOobCodeRequest
{
    public string RequestType { get; } = "PASSWORD_RESET";
    public bool ReturnOobLink { get; } = true;
    public string Email { get; set; } = null!;
    public string UserIp { get; set; } = null!;
    public string TenantId { get; set; } = null!;
    public string TargetProjectId { get; set; } = null!;
}