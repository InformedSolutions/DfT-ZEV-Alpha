using System.ComponentModel.DataAnnotations;
using DfT.ZEV.Common.Attributes;

namespace DfT.ZEV.Common.MVC.Authentication.ViewModels;

public class RequestPasswordChangeViewModel
{
    [Required(ErrorMessage = AuthErrorMessages.EmailEmpty)]
    [CustomEmailAddress(ErrorMessage = AuthErrorMessages.EmailInvalid)]
    public string Email { get; set; }
}
