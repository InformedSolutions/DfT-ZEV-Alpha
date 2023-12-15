using System.Web;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Responses;

public class GetPasswordResetTokenResponse
{
    public string Code { get; set; }    
}

public class GetOobCodeResponse
{
    public string PasswordResetToken => HttpUtility.ParseQueryString(OobLink)["oobCode"] ?? throw new InvalidOperationException("oobCode not found in OobLink");
    public string OobLink { get; set; } = null!;
}