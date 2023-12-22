using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.ManufacturerReview.Web.Controllers;

public class DataController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}