using DfT.ZEV.Common.MVC.Authentication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using DfT.ZEV.Common.MVC.Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.Logging;
using DfT.ZEV.Common.MVC.Authentication.Identity.Extensions;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Auth.Requests;

namespace DfT.ZEV.Common.MVC.Authentication.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("account")]
public partial class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IOptions<GoogleCloudConfiguration> _googleOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGoogleAuthApiClient _authApi;
    private readonly IGoogleAccountApiClient _accountApi;
    public AccountController(ILogger<AccountController> logger, IOptions<GoogleCloudConfiguration> options, IHttpContextAccessor httpContextAccessor, IGoogleAuthApiClient authApi, IGoogleAccountApiClient accountApi)
    {
        _logger = logger;
        _googleOptions = options;
        _httpContextAccessor = httpContextAccessor;
        _authApi = authApi;
        _accountApi = accountApi;
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
            var rq = new AuthorisationRequest
            {
                Email = viewModel.Email,
                Password = viewModel.Password,
                TenantId = _googleOptions.Value.Tenancy.AppTenant
            };
            
            var result = await _authApi.Authorise(rq);

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
        return View("mfa/MfaNotEnabled");
    }
    
    [Authorize]
    [HttpPost("mfa-not-enabled")]
    public async Task<IActionResult> MfaNotEnabled(MfaEnrollmentViewModel model)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var res = await _accountApi.EnrollMfa(new InitializeMFAEnrollmentRequest
            {
                IdToken = _httpContextAccessor.HttpContext.Session.GetString("Token"),
                TenantId = _googleOptions.Value.Tenancy.AppTenant,
                enrollment_info = new PhoneEnrollmentInfo
                {
                    PhoneNumber = model.PhoneNumber,
                    RecaptchaToken = null
                }
            });
        }

        return View("mfa/ContinueEnrollment");
    }
}
