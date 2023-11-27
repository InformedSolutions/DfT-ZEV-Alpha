namespace DfT.ZEV.ManufacturerReview.Application.ViewModels.Cookies;

/// <summary>
/// Cookies page view model.
/// </summary>
public class CookieViewModel
{
    public string UserConsentAnalytics { get; set; }

    public string AnalyticsTrackingCode { get; set; }

    public string RedirectPath { get; set; } = "/cookies";
}
