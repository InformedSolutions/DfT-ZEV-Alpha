using System.ComponentModel.DataAnnotations;

namespace DfT.ZEV.Common.MVC.Authentication.ViewModels;

public class ForgottenPasswordChangeViewModel
{
    public ForgottenPasswordChangeViewModel()
    {
    }

    public ForgottenPasswordChangeViewModel(string token)
    {
        Token = token;
    }

    [Required]
    public string Token { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.NewPasswordEmpty)]
    [RegularExpression(ValidationExpressions.PasswordRequirements, 
        ErrorMessage = AuthErrorMessages.PasswordNotValid)]
    public string Password { get; set; }

    [Required(ErrorMessage = AuthErrorMessages.NewPasswordEmpty)]
    [Compare(nameof(Password), ErrorMessage = AuthErrorMessages.PasswordsMustMatch)]
    public string PasswordConfirmation { get; set; }

    public void CleanPasswords()
    {
        Password = null;
        PasswordConfirmation = null;
    }
}
