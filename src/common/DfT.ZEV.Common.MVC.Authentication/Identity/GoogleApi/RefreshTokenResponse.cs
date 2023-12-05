namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class RefreshTokenResponse
{
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string IdToken { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string ProjectId { get; set; } = null!;
}