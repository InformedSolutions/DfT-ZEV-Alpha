using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Administration.Web.Controllers;

[Route("")]
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
