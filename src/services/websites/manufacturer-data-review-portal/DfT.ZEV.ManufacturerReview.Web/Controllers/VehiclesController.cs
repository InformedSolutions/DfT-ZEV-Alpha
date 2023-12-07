using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.ManufacturerReview.Web.Controllers;

[Route("vehicles")]
[Authorize]
public class VehiclesController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
