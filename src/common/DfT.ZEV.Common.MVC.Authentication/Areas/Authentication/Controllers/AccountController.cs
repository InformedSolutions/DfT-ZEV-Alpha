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

namespace DfT.ZEV.Common.MVC.Authentication.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("account")]
public partial class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IIdentityPlatform _identityPlatform;
    private readonly IOptions<GoogleCloudConfiguration> _googleOptions;

    public AccountController(ILogger<AccountController> logger, IIdentityPlatform identityPlatform, IOptions<GoogleCloudConfiguration> options)
    {
        _logger = logger;
        _identityPlatform = identityPlatform;
        _googleOptions = options;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(SignIn));
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
            var result = await _identityPlatform.AuthenticateUser(authenticationRequest);

            HttpContext.Session.SetString("Token", result.IdToken);
            HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

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
