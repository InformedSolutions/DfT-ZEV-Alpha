using System.Web;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.PasswordChange;

public class PasswordResetTokenResponse
{
  public string PasswordResetToken => HttpUtility.ParseQueryString(OobLink)["oobCode"] ?? throw new InvalidOperationException("oobCode not found in OobLink");
  public string OobLink { get; set; } = null!;
}