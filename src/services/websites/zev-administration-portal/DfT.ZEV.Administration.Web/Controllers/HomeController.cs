using System.Threading.Tasks;
using DfT.ZEV.Common.Services.Clients;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Administration.Web.Controllers;

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
