using DfT.ZEV.Administration.Application.ViewModels.Cookies;
using DfT.ZEV.Administration.Web.Extensions.TagHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace DfT.ZEV.Administration.Web.Controllers;

[Route("cookies")]
public class CookiesController : Controller
{
    private readonly GoogleAnalyticsOptions _googleAnalyticsOptions;

    public CookiesController(IOptions<GoogleAnalyticsOptions> googleAnalyticsOptions)
    {
        _googleAnalyticsOptions = googleAnalyticsOptions.Value;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new CookieViewModel
        {
            AnalyticsTrackingCode = _googleAnalyticsOptions.TrackingCode,
        };

        viewModel.UserConsentAnalytics = HttpContext.Features.Get<ITrackingConsentFeature>().HasConsent ? "accept" : "reject";

        if (!TempData.ContainsKey("RefererUrl"))
        {
            TempData["RefererUrl"] = Request.GetTypedHeaders().Referer?.ToString();
        }

        return View(viewModel);
    }

    [HttpPost("set-consent")]
    public IActionResult SetConsent(CookieViewModel viewModel)
    {
        if (viewModel.UserConsentAnalytics == "accept")
        {
            HttpContext.Features.Get<ITrackingConsentFeature>().GrantConsent();
        }
        else if (viewModel.UserConsentAnalytics == "reject")
        {
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(1),
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = HttpContext.Request.IsHttps,
                Path = "/",
            };

            HttpContext.Response.Cookies.Append("AdditionalCookiesConsent", "no", cookieOptions);

            ExpireAnalyticsCookies();
        }

        TempData["UserConsentSetInPreviousRequest"] = true;

        return Redirect(viewModel.RedirectPath);
    }

    /// <summary>
    /// Helper method to expire analytics cookie (so that they get deleted by a client browser).
    /// </summary>
    private void ExpireAnalyticsCookies()
    {
        // If user has previously consented and now chosen not to - delete old GA cookies by expiring them.
        foreach (var cookie in HttpContext.Request.Cookies)
        {
            if (cookie.Key.Contains("_ga") || cookie.Key.Contains("_gid") || cookie.Key.Contains("_gat"))
            {
                // Note that manually constructing cookie options is needed here, as the domain stamped by GA includes a prefixed .
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Secure = false,
                    Path = "/",
                };

                // To allow clearing of cookies, the same domain needs set. GA appends a . prefix to the domain in cases
                // where a service is not running on a local address.
                if (HttpContext.Request.Host.Host != "localhost" && HttpContext.Request.Host.Host != "127.0.0.1")
                {
                    cookieOptions.Domain = $".{HttpContext.Request.Host.Host}";
                }

                HttpContext.Response.Cookies.Append(cookie.Key, string.Empty, cookieOptions);
            }
        }
    }
}
