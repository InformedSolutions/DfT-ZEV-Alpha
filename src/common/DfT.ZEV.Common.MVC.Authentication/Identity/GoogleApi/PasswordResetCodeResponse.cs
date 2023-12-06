namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;
using System.Web;
public class PasswordResetCodeResponse
{
  public string OobCode => HttpUtility.ParseQueryString(OobLink)["oobCode"] ?? throw new InvalidOperationException("oobCode not found in OobLink");
  public string OobLink { get; set; } = null!;
}