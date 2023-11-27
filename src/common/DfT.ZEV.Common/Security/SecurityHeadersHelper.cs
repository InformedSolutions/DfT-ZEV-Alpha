using DfT.ZEV.Common.ContentSecurityPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace DfT.ZEV.Common.Security;

/// <summary>
/// Helper class for dealing with HTTP response headers concerning security.
/// </summary>
public static class SecurityHeadersHelper
{
    /// <summary>
    /// Helper method to set common security headers.
    /// </summary>
    /// <param name="ctx">HttpContext to be configured.</param>
    /// <param name="additionalCspSourceValues">Additional optional CSP source values to be injected.</param>
    /// <param name="allowUnsafeInlineStyles">Whether unsafe inline styles are to be permitted.</param>
    public static void ConfigureCommonSecurityHeaders(HttpContext ctx, string additionalCspSourceValues = null, bool allowUnsafeInlineStyles = false)
    {
        if (string.IsNullOrEmpty(additionalCspSourceValues))
        {
            ctx.Response.Headers.Add(
            HeaderNames.ContentSecurityPolicy,
            ContentSecurityPolicyHeadersHelper.GetDefaultCSP(allowUnsafeInlineStyles).ToString());
        }
        else
        {
            ctx.Response.Headers.Add(
                HeaderNames.ContentSecurityPolicy,
                ContentSecurityPolicyHeadersHelper.GetCSPWithAdditionalValues(additionalCspSourceValues, allowUnsafeInlineStyles).ToString());
        }

        ctx.Response.Headers.Add("Cross-Origin-Embedder-Policy", "unsafe-none");
        ctx.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");
        ctx.Response.Headers.Add("Cross-Origin-Resource-Policy", "cross-origin");
        ctx.Response.Headers.Add("permissions-policy", "geolocation=(), midi=(), sync-xhr=(), microphone=(), camera=(), magnetometer=(), gyroscope=(), fullscreen=(), payment=()");
        ctx.Response.Headers.Add("Referrer-Policy", "no-referrer");
        ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        ctx.Response.Headers.Add(HeaderNames.XFrameOptions, "DENY");
        ctx.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
        ctx.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
        ctx.Response.Headers.Add("X-UA-Compatible", "IE=Edge");

        // Note this is used as a fallback from app.useHsts() as this helper only applies when accessed over https.
        // In a load balanced scenario where SSL is terminated at the ALB this would not apply so needs manually set.
        ctx.Response.Headers.Add(HeaderNames.StrictTransportSecurity, "max-age=31536000");

        // Caching pragmas are set here to avoid security issues if someone forgets to append the ResponseCache attribute to
        // a controller method
        ctx.Response.Headers.Add(HeaderNames.CacheControl, "no-store,no-cache,must-revalidate");
        ctx.Response.Headers.Add(HeaderNames.Pragma, "no-cache");
    }
}
