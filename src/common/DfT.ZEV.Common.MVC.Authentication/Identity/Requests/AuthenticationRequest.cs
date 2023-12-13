namespace DfT.ZEV.Common.MVC.Authentication.Identity.Requests;

public class AuthenticationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

    public AuthenticationRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
}