namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class PasswordChangeWithTokenRequest
{
  public string PasswordResetToken { get; set; }
  public string NewPassword { get; set; }
  public string TenantId { get; set; }
}