using DfT.ZEV.Common.MVC.Authentication.Attributes;
using DfT.ZEV.Common.MVC.Authentication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.MVC.Authentication.Areas.Authentication.Controllers;

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

        //await _resetPasswordService.RequestForgottenPasswordChange(viewModel);

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
        //var result = await _resetPasswordService.VerifyForgottenPasswordToken(token);
        //return result.FailedWithRedirect
        //    ? RedirectToAction(nameof(ForgottenPasswordFailed))
        //    : View("ForgottenPassword/ChangeForgottenPassword", new ForgottenPasswordChangeViewModel(token));

        return View("ForgottenPassword/ChangeForgottenPassword", new ForgottenPasswordChangeViewModel(token));
    }

    [UserPasswordManagement]
    [HttpPost("change-forgotten-password")]
    public async Task<IActionResult> ChangeForgottenPassword(ForgottenPasswordChangeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("ForgottenPassword/ChangeForgottenPassword", viewModel);
        }

        // var result = await _resetPasswordService.ChangeForgottenPassword(viewModel);
        // if (result.FailedWithRedirect)
        // {
        //     return RedirectToAction(nameof(ForgottenPasswordFailed));
        // }
        //
        // if (!result.Succeeded)
        // {
        //     foreach (var (key, value) in result.Errors)
        //     {
        //         ModelState.AddModelError(key, value);
        //     }
        //
        //     return View("ForgottenPassword/ChangeForgottenPassword", viewModel);
        // }

        return RedirectToAction(nameof(SignIn), "Account", new { message = "PasswordChangedSuccess" });
    }
    #endregion

    #region SetInitialPassword
    [UserPasswordManagement]
    [HttpGet("set-initial-password/{oobCode}")]
    public IActionResult SetInitialPassword(string oobCode)
    {
        var email = TempData["UserEmailSignInAttempt"]?.ToString();

        return View(new SetInitialPasswordViewModel() { OobCode = oobCode });
    }

    [UserPasswordManagement]
    [HttpPost("set-initial-password/{oobCode}")]
    public async Task<IActionResult> SetInitialPassword(string oobCode, SetInitialPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            await _identityPlatform.ChangePasswordAsync(viewModel.OobCode, viewModel.Password, _googleOptions.Value.Tenancy.AppTenant);
            return RedirectToAction(nameof(SignIn), "Account", new { message = "PasswordChangedSuccess" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Could not set initial password. Please try again.");
            _logger.LogError(ex, ex.Message);
            return View(viewModel);
        }

    }
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

    #endregion

    #region ChangePassword
    [UserPasswordManagement]
    [Authorize]
    [HttpGet("change-password")]
    public IActionResult ChangePassword()
    {
        return View();
    }

    #endregion

    #region AccountActivationSetInitialPassword
    [UserPasswordManagement]
    [HttpGet("account-activation-set-initial-password")]
    public async Task<IActionResult> AccountActivationSetInitialPassword([FromQuery] string oobCode)
    {
        return View("AccountActivationSetInitialPassword", new ForgottenPasswordChangeViewModel(oobCode));
    }

    [UserPasswordManagement]
    [HttpPost("account-activation-set-initial-password")]
    public async Task<IActionResult> AccountActivationSetInitialPassword(ForgottenPasswordChangeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("AccountActivationSetInitialPassword", viewModel);
        }

        return RedirectToAction(nameof(SignIn), "Account", new { message = "PasswordChangedSuccess" });
    }
    #endregion
}
