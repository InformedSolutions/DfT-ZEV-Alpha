namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public class AuthorizationResponse
{
    public string Kind { get; set; } = null!;
    public Guid LocalId { get; set; }
    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string IdToken { get; set; } = null!;
    public bool Registered { get; set; }
    public string RefreshToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
}