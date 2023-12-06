namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class PasswordChangeRequest
{
  public string OobCode { get; set; }
  public string NewPassword { get; set; }
  public string TenantId { get; set; }
}