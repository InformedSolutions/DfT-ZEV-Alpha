namespace DfT.ZEV.Common.ContentSecurityPolicy;

/// <summary>
/// Helper utility for configuring security headers based on common best-practice.
/// </summary>
public class ContentSecurityPolicyHeadersHelper
{
    /// <summary>
    /// Returns a default CSP based on typical best-practice values.
    /// </summary>
    /// <param name="allowUnsafeInlineStyles">Option to allow users to opt-out of blocking inline styles.</param>
    /// <returns>A configured content security policy.</returns>
    public static CspOptions GetDefaultCSP(bool allowUnsafeInlineStyles = false)
    {
        var csp = new CspOptions();
        csp.Default.AllowSelf();
        csp.Script.AllowSelf();
        csp.Style.AllowSelf();

        if (allowUnsafeInlineStyles)
        {
            csp.Style.AllowUnsafeInline();
        }

        csp.StyleElement.AllowSelf();
        csp.Image.AllowSelf();
        csp.Connect.AllowSelf();
        csp.Frame.AllowSelf();
        csp.Form.AllowSelf();
        return csp;
    }

    /// <summary>
    /// Returns a default CSP based on typical best-practice values with additional source values injected.
    /// </summary>
    /// <param name="additionalCspSourceValues">Additional source values to permit in the CSP result.</param>
    /// <param name="allowUnsafeInlineStyles">Option to allow users to opt-out of blocking inline styles.</param>
    /// <returns>A configured content security policy.</returns>
    public static CspOptions GetCSPWithAdditionalValues(string additionalCspSourceValues, bool allowUnsafeInlineStyles = false)
    {
        var csp = GetDefaultCSP(allowUnsafeInlineStyles);
        csp.Default.Allow(additionalCspSourceValues);
        csp.Script.Allow(additionalCspSourceValues);
        csp.StyleElement.Allow(additionalCspSourceValues);
        csp.Image.Allow(additionalCspSourceValues);
        csp.Connect.Allow(additionalCspSourceValues);
        csp.Form.Allow(additionalCspSourceValues);
        return csp;
    }
}
