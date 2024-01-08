using System.ComponentModel.DataAnnotations;

namespace DfT.ZEV.Common.MVC.Authentication.ViewModels;

public class ChangePasswordViewModel
{
    public ChangePasswordViewModel()
    {
    }

    [Required(ErrorMessage = AuthErrorMessages.OldPasswordEmpty)]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.NewPasswordEmpty)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{12,}$", 
        ErrorMessage = AuthErrorMessages.PasswordNotValid)]
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
