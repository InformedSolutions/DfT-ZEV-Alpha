namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class RefreshTokenRequest
{
    public string GrantType { get; set; } = "refresh_token";
    public string RefreshToken { get; set; } = null!;
}