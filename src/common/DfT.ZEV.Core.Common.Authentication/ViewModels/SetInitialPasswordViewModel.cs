using System.ComponentModel.DataAnnotations;

namespace DfT.ZEV.Core.Common.Authentication.ViewModels;

public class SetInitialPasswordViewModel
{
    public SetInitialPasswordViewModel()
    {
    }

    public SetInitialPasswordViewModel(string email)
    {
        Email = email;
    }

    [Required]
    public string Email { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.TemporaryPasswordEmpty)]
    public string TemporaryPassword { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.NewPasswordEmpty)]
    public string Password { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.NewPasswordEmpty)]
    [Compare(nameof(Password), ErrorMessage = AuthErrorMessages.PasswordsMustMatch)]
    public string PasswordConfirmation { get; set; }

    public void CleanPasswords()
    {
        TemporaryPassword = null;
        Password = null;
        PasswordConfirmation = null;
    }
}
