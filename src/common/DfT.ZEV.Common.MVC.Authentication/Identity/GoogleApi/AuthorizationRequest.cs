namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class AuthorizationRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool ReturnSecureToken { get; set; } = true;
    public string TenantId { get; set; } = null!;
}