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

        try
        {
            var resetToken = await _identityPlatform.GetPasswordResetToken(viewModel.Email, _googleOptions.Value.Tenancy.AppTenant);
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            _logger.LogInformation($"DEMO LINK FOR EMAIL: {host}/account/change-forgotten-password/{resetToken}");
        }
        catch(Exception ex)
        {
            // ignore failed operation result not to reveal user existence in db.
            _logger.LogError(ex, ex.Message);
        }

        return View("ForgottenPassword/ForgottenPasswordEmailSent", viewModel.Email);
    }

    [UserPasswordManagement]
    [HttpGet("forgotten-password-failed")]
    public IActionResult ForgottenPasswordFailed()
    {
        return View("ForgottenPassword/ForgottenPasswordFailed");
    }

    [UserPasswordManagement]
    [HttpGet("change-forgotten-password/{passwordResetToken}")]
    public async Task<IActionResult> ChangeForgottenPassword(string passwordResetToken)
    {
        return View("ForgottenPassword/ChangeForgottenPassword", new ForgottenPasswordChangeViewModel(passwordResetToken));
    }

    [UserPasswordManagement]
    [HttpPost("change-forgotten-password/{passwordResetToken}")]
    public async Task<IActionResult> ChangeForgottenPassword(ForgottenPasswordChangeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("ForgottenPassword/ChangeForgottenPassword", viewModel);
        }

        await _identityPlatform.ChangePasswordAsync(viewModel.Token, viewModel.Password, _googleOptions.Value.Tenancy.AppTenant);

        return RedirectToAction(nameof(SignIn), "Account", new { message = "PasswordChangedSuccess" });
    }
    #endregion

    #region SetInitialPassword
    [UserPasswordManagement]
    [HttpGet("set-initial-password/{passwordResetToken}")]
    public IActionResult SetInitialPassword(string passwordResetToken)
    {
        var email = TempData["UserEmailSignInAttempt"]?.ToString();

        return View(new SetInitialPasswordViewModel() { OobCode = passwordResetToken });
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
