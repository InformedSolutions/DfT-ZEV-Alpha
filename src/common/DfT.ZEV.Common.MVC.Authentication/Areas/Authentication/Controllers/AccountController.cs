using DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;
using DfT.ZEV.Common.MVC.Authentication.ViewModels;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.MVC.Authentication.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("account")]
public partial class AccountController : Controller
{
    private readonly IIdentityPlatform _identityPlatform;
    private readonly ILogger<AccountController> _logger;
    public AccountController(IIdentityPlatform identityPlatform, ILogger<AccountController> logger)
    {
        _identityPlatform = identityPlatform;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(SignIn));
    }

    [HttpGet("sign-in")]
    public IActionResult SignIn([FromQuery] string message)
    {
        //if (User.Identity.IsAuthenticated
        //    && User.Identity.AuthenticationType != BasicAuthenticationDefaults.AuthenticationScheme)
        //{
        //    return Redirect("/");
        //}

        if (!string.IsNullOrEmpty(message))
        {
            ViewData["message"] = message;
        }

        return View(new SignInViewModel());
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel, [FromQuery] string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            viewModel.CleanPassword();
            return View(viewModel);
        }
        var result = await _identityPlatform.AuthorizeUser(viewModel.Email, viewModel.Password);
        _logger.LogInformation("User {Email} signed in successfully", viewModel.Email);
        //var result = await _signInService.SignIn(viewModel);

        /*if (result.ForceInitialPasswordSet)
        {
            TempData["UserEmailSignInAttempt"] = viewModel.Email;
            return RedirectToAction(nameof(SetInitialPassword));
        }

        if (result.ForcePasswordChange)
        {
            TempData["UserEmailSignInAttempt"] = viewModel.Email;
            return RedirectToAction(nameof(ChangeExpiredPassword));
        }

        if (!result.Succeeded)
        {
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }

            viewModel.CleanPassword();
            return View(viewModel);
        }*/

        return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Home");
    }

    [HttpGet("sign-out")]
    public IActionResult SignOutPage()
    {
        return View();
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> Logout()
    {
        //await _signOutService.SignOut();

        return RedirectToAction(nameof(SignOutPage));
    }

    [Authorize]
    [HttpPost("save-return-url")]
    public IActionResult SaveReturnUrl([FromBody] SessionReturnUrlViewModel viewModel)
    {
        HttpContext.Session.Set("SessionTimeoutReturnUrl", Encoding.UTF8.GetBytes(viewModel.ReturnUrl));

        return Ok();
    }

    [Authorize]
    [HttpGet("session-expiry")]
    public IActionResult SessionExpiry()
    {
        return View();
    }

    [Authorize]
    [HttpPost("extend-session")]
    public IActionResult ExtendSession()
    {
        var returnUrl = "/";
        if (HttpContext.Session.TryGetValue("SessionTimeoutReturnUrl", out var returnUrlBytes))
        {
            returnUrl = Encoding.UTF8.GetString(returnUrlBytes);
            HttpContext.Session.Remove("SessionTimeoutReturnUrl");
        }

        return Redirect(returnUrl);
    }
}
