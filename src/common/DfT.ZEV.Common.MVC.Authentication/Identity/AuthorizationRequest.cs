namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public class AuthorizationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

    public AuthorizationRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
}