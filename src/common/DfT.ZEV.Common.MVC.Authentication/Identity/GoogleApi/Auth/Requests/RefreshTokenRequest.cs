namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth.Requests;

public class RefreshTokenRequest
{
    public string GrantType { get; set; } = "refresh_token";
    public string RefreshToken { get; set; } = null!;
}