using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Administration.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
