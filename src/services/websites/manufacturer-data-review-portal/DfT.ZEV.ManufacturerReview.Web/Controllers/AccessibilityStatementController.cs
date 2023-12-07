using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.ManufacturerReview.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
