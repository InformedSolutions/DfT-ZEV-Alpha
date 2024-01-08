namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public class PasswordChangeWithPasswordRequest
{
    public string NewPassword { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string TenantId { get; set; } = null!;
}