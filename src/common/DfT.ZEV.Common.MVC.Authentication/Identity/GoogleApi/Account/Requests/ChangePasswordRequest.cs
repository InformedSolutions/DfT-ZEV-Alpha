namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;

public class ChangePasswordRequest
{
    public string OobCode { get; set; }
    public string NewPassword { get; set; }
    public string TenantId { get; set; }
}