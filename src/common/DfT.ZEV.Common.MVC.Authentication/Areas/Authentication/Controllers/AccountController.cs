using DfT.ZEV.Common.MVC.Authentication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Common.MVC.Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.Logging;
using DfT.ZEV.Common.MVC.Authentication.Identity.Extensions;
using DfT.ZEV.Common.MVC.Authentication.Identity.Interfaces;
using DfT.ZEV.Common.MVC.Authentication.Identity.Requests;

namespace DfT.ZEV.Common.MVC.Authentication.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("account")]
public partial class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IIdentityPlatform _identityPlatform;
    private readonly IOptions<GoogleCloudConfiguration> _googleOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(ILogger<AccountController> logger, IIdentityPlatform identityPlatform, IOptions<GoogleCloudConfiguration> options, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _identityPlatform = identityPlatform;
        _googleOptions = options;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(SignIn));
    }

    [HttpGet("sign-in")]
    public IActionResult SignIn([FromQuery] string message)
    {
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

        try
        {
            var authenticationRequest = new AuthenticationRequest(viewModel.Email, viewModel.Password);
            var result = await _identityPlatform.AuthenticateUser(authenticationRequest, _googleOptions.Value.Tenancy.AppTenant);

            HttpContext.Session.SetString("Token", result.IdToken);
            HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

            _logger.LogBusinessEvent("User successfully signed in");
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing in user {Email}", viewModel.Email);
            viewModel.CleanPassword();
            ModelState.AddModelError(string.Empty, "The email or password you entered is incorrect");
            return View();
        }
    }

    [HttpGet("details")]
    public IActionResult Details()
    {
        var claims = User.Claims.ToList();
        if (claims.Any())
        {
            return View(new AccountDetails()
            {
                IdentityAccountDetails = User.GetAccountDetails()
            });
        }

        return Redirect("~/");
    }

    [HttpGet("sign-out")]
    public IActionResult SignOutPage()
    {
        HttpContext.Session.Remove("Token");
        HttpContext.Session.Remove("RefreshToken");

        return View();
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Remove("Token");
        HttpContext.Session.Remove("RefreshToken");
        return RedirectToAction(nameof(SignOutPage));
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

    [Authorize]
    [HttpGet("mfa-not-enabled")]
    public async Task<IActionResult> MfaNotEnabled()
    {
        return View();
    }
}
