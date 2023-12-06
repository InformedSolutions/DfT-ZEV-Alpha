namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class PasswordResetCodeRequest
{
  public string RequestType { get; } = "PASSWORD_RESET";
  public bool ReturnOobLink { get; } = true;

  public string UserIp { get; set; } = null!;
  public string ContinueUrl { get; set; } = null!;
  public string TenantId { get; set; } = null!;
  public string TargetProjectId { get; set; } = null!;

}