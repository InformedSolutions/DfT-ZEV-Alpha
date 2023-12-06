using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.ManufacturerReview.Web.Controllers;

[Route("")]
[Authorize]
public class HomeController : Controller
{
    public HomeController()
    {
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [Route("accessibility-statement")]
    [HttpGet]
    public IActionResult AccessibilityStatement()
    {
        return View();
    }
}
