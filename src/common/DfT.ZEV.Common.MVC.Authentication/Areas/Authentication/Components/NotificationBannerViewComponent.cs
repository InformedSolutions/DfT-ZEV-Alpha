using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Common.MVC.Authentication.Components;

public class NotificationBannerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string bannerType, string body, string redirectUrl = null)
    {
        return View("NotificationBanner", new NotificationBannerViewComponentModel(bannerType, body, redirectUrl));
    }
}
