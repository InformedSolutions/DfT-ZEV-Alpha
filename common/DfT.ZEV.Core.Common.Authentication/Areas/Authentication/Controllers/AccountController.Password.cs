using DfT.ZEV.Core.Common.Authentication.Attributes;
using DfT.ZEV.Core.Common.Authentication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Core.Common.Authentication.Areas.Authentication.Controllers;

public partial class AccountController : Controller
{
    #region ForgottenPassword
    [UserPasswordManagement]
    [HttpGet("forgotten-password")]
    public IActionResult ForgottenPassword()
    {
        return View("ForgottenPassword/ForgottenPassword");
    }

    [UserPasswordManagement]
    [HttpPost("forgotten-password")]
    public async Task<IActionResult> ForgottenPassword(RequestPasswordChangeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("ForgottenPassword/ForgottenPassword", viewModel);
        }

        await _resetPasswordService.RequestForgottenPasswordChange(viewModel);

        // ignore failed operation result not to reveal user existence in db.
        return View("ForgottenPassword/ForgottenPasswordEmailSent", viewModel.Email);
    }

    [UserPasswordManagement]
    [HttpGet("forgotten-password-failed")]
    public IActionResult ForgottenPasswordFailed()
    {
        return View("ForgottenPassword/ForgottenPasswordFailed");
    }

    [UserPasswordManagement]
    [HttpGet("change-forgotten-password")]
    public async Task<IActionResult> ChangeForgottenPassword([FromQuery] string token)
    {
        var result = await _resetPasswordService.VerifyForgottenPasswordToken(token);
        return result.FailedWithRedirect
            ? RedirectToAction(nameof(ForgottenPasswordFailed))
            : View("ForgottenPassword/ChangeForgottenPassword", new ForgottenPasswordChangeViewModel(token));
    }

    [UserPasswordManagement]
    [HttpPost("change-forgotten-password")]
    public async Task<IActionResult> ChangeForgottenPassword(ForgottenPasswordChangeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("ForgottenPassword/ChangeForgottenPassword", viewModel);
        }

        var result = await _resetPasswordService.ChangeForgottenPassword(viewModel);
        if (result.FailedWithRedirect)
        {
            return RedirectToAction(nameof(ForgottenPasswordFailed));
        }

        if (!result.Succeeded)
        {
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }

            return View("ForgottenPassword/ChangeForgottenPassword", viewModel);
        }

        return RedirectToAction(nameof(AccountController.SignIn), "Account", new { message = "PasswordChangedSuccess" });
    }
    #endregion

    #region SetInitialPassword
    [UserPasswordManagement]
    [HttpGet("set-initial-password")]
    public IActionResult SetInitialPassword()
    {
        var email = TempData["UserEmailSignInAttempt"]?.ToString();

        return View(new SetInitialPasswordViewModel(email));
    }

    // [UserPasswordManagement]
    // [HttpPost("set-initial-password")]
    // public async Task<IActionResult> SetInitialPassword(SetInitialPasswordViewModel viewModel)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(viewModel);
    //     }

    //     var result = await _passwordService.SetInitialPassword(viewModel);
    //     if (result.FailedWithRedirect)
    //     {
    //         // TODO check if this page is appropriate
    //         return RedirectToAction(nameof(ForgottenPasswordFailed));
    //     }

    //     if (!result.Succeeded)
    //     {
    //         foreach (var (key, value) in result.Errors)
    //         {
    //             ModelState.AddModelError(key, value);
    //         }

    //         return View(viewModel);
    //     }

    //     return RedirectToAction(nameof(AccountController.SignIn), "Account", new { message = "PasswordChangedSuccess" });
    // }
    #endregion

    #region ChangeExpiredPassword
    [UserPasswordManagement]
    [HttpGet("change-expired-password")]
    public IActionResult ChangeExpiredPassword()
    {
        var email = TempData["UserEmailSignInAttempt"]?.ToString();
        return email is null
            ? RedirectToAction(nameof(AccountController.SignIn), "Account")
            : View(new ChangeExpiredPasswordViewModel(email));
    }

    // [UserPasswordManagement]
    // [HttpPost("change-expired-password")]
    // public async Task<IActionResult> ChangeExpiredPassword(ChangeExpiredPasswordViewModel viewModel)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(viewModel);
    //     }

    //     var result = await _passwordService.ChangeExpiredPassword(viewModel);

    //     if (result.FailedWithRedirect)
    //     {
    //         return RedirectToAction(nameof(SignIn));
    //     }

    //     if (!result.Succeeded)
    //     {
    //         foreach (var (key, value) in result.Errors)
    //         {
    //             ModelState.AddModelError(key, value);
    //         }

    //         return View(viewModel);
    //     }

    //     return RedirectToAction(nameof(AccountController.SignIn), "Account", new { message = "PasswordChangedSuccess" });
    // }
    #endregion

    #region ChangePassword
    [UserPasswordManagement]
    [Authorize]
    [HttpGet("change-password")]
    public IActionResult ChangePassword()
    {
        return View();
    }

    // [UserPasswordManagement]
    // [Authorize]
    // [HttpPost("change-password")]
    // public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(viewModel);
    //     }

    //     var result = await _passwordService.ChangePassword(viewModel);

    //     if (result.ForceLogout)
    //     {
    //         await _signOutService.SignOut();
    //         return RedirectToAction(nameof(AccountController.SignIn), "Account", new { message = "LogoutDueToSecurity" });
    //     }

    //     if (!result.Succeeded)
    //     {
    //         foreach (var (key, value) in result.Errors)
    //         {
    //             ModelState.AddModelError(key, value);
    //         }

    //         return View(viewModel);
    //     }

    //     await _signOutService.SignOut();
    //     return RedirectToAction(nameof(AccountController.SignIn), "Account", new { message = "PasswordChangedSuccess" });
    // }
    #endregion
    
    #region AccountActivationSetInitialPassword
    [UserPasswordManagement]
    [HttpGet("account-activation-set-initial-password")]
    public async Task<IActionResult> AccountActivationSetInitialPassword([FromQuery] string token)
    {
        var result = await _resetPasswordService.VerifyForgottenPasswordToken(token);
        return result.FailedWithRedirect
            ? RedirectToAction(nameof(ForgottenPasswordFailed))
            : View("AccountActivationSetInitialPassword", new ForgottenPasswordChangeViewModel(token));
    }

    [UserPasswordManagement]
    [HttpPost("account-activation-set-initial-password")]
    public async Task<IActionResult> AccountActivationSetInitialPassword(ForgottenPasswordChangeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("AccountActivationSetInitialPassword", viewModel);
        }

        var result = await _resetPasswordService.ChangeForgottenPassword(viewModel);
        if (result.FailedWithRedirect)
        {
            return RedirectToAction(nameof(ForgottenPasswordFailed));
        }

        if (!result.Succeeded)
        {
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }

            return View("ForgottenPassword/ChangeForgottenPassword", viewModel);
        }

        return RedirectToAction(nameof(AccountController.SignIn), "Account", new { message = "PasswordChangedSuccess" });
    }
    #endregion
}
