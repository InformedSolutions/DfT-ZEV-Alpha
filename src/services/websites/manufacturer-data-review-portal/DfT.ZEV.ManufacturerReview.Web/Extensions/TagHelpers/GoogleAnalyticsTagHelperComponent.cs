using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;

namespace DfT.ZEV.ManufacturerReview.Web.Extensions.TagHelpers;

/// <summary>
/// Tag helper for rendering Google Analytics scripts.
/// </summary>
public class GoogleAnalyticsTagHelperComponent : TagHelperComponent
{
    private readonly GoogleAnalyticsOptions _googleAnalyticsOptions;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public GoogleAnalyticsTagHelperComponent(IOptions<GoogleAnalyticsOptions> googleAnalyticsOptions, IHttpContextAccessor httpContextAccessor)
    {
        _googleAnalyticsOptions = googleAnalyticsOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var consentCookie = _httpContextAccessor.HttpContext.Request.Cookies["AdditionalCookiesConsent"];

        if (consentCookie != null && consentCookie.Equals("yes"))
        {
            // Inject the code only in the head element
            if (string.Equals(output.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                // Get the tracking code from the configuration
                var trackingCode = _googleAnalyticsOptions.TrackingCode;
                if (!string.IsNullOrEmpty(trackingCode))
                {
                    string cookieFlags = (_httpContextAccessor.HttpContext.Request.IsHttps || _httpContextAccessor.HttpContext.Request.Host.Host == "localhost" || _httpContextAccessor.HttpContext.Request.Host.Host == "127.0.0.1")
                        ? "samesite=strict; secure"
                        : "samesite=strict;";

                    var injectedJs = "<script async nonce='owned-assets' src='https://www.googletagmanager.com/gtag/js?id=" + trackingCode + "'></script>"
                        + "<script nonce='owned-assets'>"
                            + "window.dataLayer=window.dataLayer||[];function gtag(){dataLayer.push(arguments)}gtag('js',new Date);"
                            + "gtag('config','" + trackingCode + "',{displayFeaturesTask:'null', cookie_flags:'" + cookieFlags + "'});"
                        + "</script>";

                    output.PostContent.AppendHtml(injectedJs);
                }
            }
        }
        else
        {
            InjectJsGATrackingFallback(output);
        }
    }

    /// <summary>
    /// Helper method to expire analytics cookie (so that they get deleted by a client browser).
    /// </summary>
    private void InjectJsGATrackingFallback(TagHelperOutput output)
    {
        // As a fallback disable GA at the window level (see https://developers.google.com/analytics/devguides/collection/analyticsjs/user-opt-out)
        var analyticsContainerId = _googleAnalyticsOptions.TrackingCode.Substring(_googleAnalyticsOptions.TrackingCode.IndexOf("-") + 1);
        var containerIdCookieName = $"_ga_{analyticsContainerId}";

        // Some browsers do not respect the removal of cookies using the Set-Cookie header, so this JS insert is used as a fallback to forcibly
        // remove GA cookies on page load by setting their expiry to the unix epoch
        output.PostContent
                .AppendHtml("<script nonce='owned-assets'>")
                .AppendHtml($"window['ga-disable-{_googleAnalyticsOptions.TrackingCode}'] = true;")
                .AppendHtml("</script>");
    }
}
