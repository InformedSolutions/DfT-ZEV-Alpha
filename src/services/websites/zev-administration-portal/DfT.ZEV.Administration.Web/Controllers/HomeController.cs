using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DfT.ZEV.Administration.Web.Controllers;

[Authorize]
[Route("")]
public class HomeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
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
