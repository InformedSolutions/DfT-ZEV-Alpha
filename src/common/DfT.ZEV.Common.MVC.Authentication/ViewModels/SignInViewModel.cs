using System.ComponentModel.DataAnnotations;
using DfT.ZEV.Common.Attributes;

namespace DfT.ZEV.Common.MVC.Authentication.ViewModels;

public class SignInViewModel
{
    [Required(ErrorMessage = AuthErrorMessages.EmailEmpty)]
    [CustomEmailAddress(ErrorMessage = AuthErrorMessages.EmailInvalid)]
    public string Email { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.PasswordEmpty)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets HoneyPotCaptcha
    /// This is not UserId. This field works as honeypot captcha. Should be implemented as hidden input.
    /// If we receive request with this field filled then there is high probability bot has interacted with the app.
    /// </summary>
    public string UserId { get; set; }

    public void CleanPassword()
    {
        Password = null;
    }
}
