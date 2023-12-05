using DfT.ZEV.Common.MVC.Authentication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Common.MVC.Authentication.Models;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("details")]
    public IActionResult Details()
    {
        var claims = User.Claims.ToList();
        if (claims.Any())
        {
            return this.View(new AccountDetails()
            {
                Email = claims.FirstOrDefault(x => x.Type == "email")?.Value
            });
        }
        
        return Redirect("~/");
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

        try
        {
            var result = await _identityPlatform.AuthorizeUser(viewModel.Email, viewModel.Password);
            HttpContext.Session.SetString("Token",result.IdToken);
            HttpContext.Session.SetString("RefreshToken",result.RefreshToken);

            return RedirectToAction("Index", "Home");

        }
        catch (Exception ex)
        {
            viewModel.CleanPassword();
            ViewData["message"] = "InvalidCredentials";

            return View();
        }
        _logger.LogInformation("User {Email} signed in successfully", viewModel.Email);
        //var result = await _signInService.SignIn(viewModel);

        /*if (result.ForceInitialPasswordSet)
        {
            TempData["UserEmailSignInAttempt"] = viewModel.Email;
            return RedirectToAction(nameof(SetInitialPassword));
        }*/

        //return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Home");
        //return RedirectToPage("/Index");
        
        return RedirectToAction("Index", "Home");
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
        //await _signOutService.SignOut();
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
