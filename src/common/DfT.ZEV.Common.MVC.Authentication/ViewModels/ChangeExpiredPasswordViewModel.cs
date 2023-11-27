using System.ComponentModel.DataAnnotations;

namespace DfT.ZEV.Common.MVC.Authentication.ViewModels;

public class ChangeExpiredPasswordViewModel
{
    public ChangeExpiredPasswordViewModel()
    {
    }

    public ChangeExpiredPasswordViewModel(string email)
    {
        Email = email;
    }

    [Required]
    public string Email { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.OldPasswordEmpty)]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.NewPasswordEmpty)]
    public string Password { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.NewPasswordEmpty)]
    [Compare(nameof(Password), ErrorMessage = AuthErrorMessages.PasswordsMustMatch)]
    public string PasswordConfirmation { get; set; }

    public void CleanPasswords()
    {
        OldPassword = null;
        Password = null;
        PasswordConfirmation = null;
    }
}
