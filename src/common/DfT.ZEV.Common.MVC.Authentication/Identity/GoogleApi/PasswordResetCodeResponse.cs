namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class PasswordResetCodeResponse
{
  public string OobCode { get; set; } = null!;
  public string OobLink { get; set; } = null!;
}