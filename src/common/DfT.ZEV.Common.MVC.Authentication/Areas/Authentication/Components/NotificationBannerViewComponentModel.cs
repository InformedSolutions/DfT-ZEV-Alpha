namespace DfT.ZEV.Common.MVC.Authentication.Areas.Authentication.Components;

public class NotificationBannerViewComponentModel
{
    public NotificationBannerViewComponentModel()
    {
    }

    public NotificationBannerViewComponentModel(string bannerType, string body, string redirectUrl = null)
    {
        BannerType = bannerType;
        Body = body;
        RedirectUrl = redirectUrl;
    }

    public string BannerType { get; }

    public string Body { get; }

    public string RedirectUrl { get; }
}
