using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DfT.ZEV.ManufacturerReview.Web.Controllers;

/// <summary>
/// Returns a build ID from a configured environment variable. This is used in deployment pipelines
/// so that checks can be made that roll-out tasks have completed before running automated tests.
/// This can also be used to trace back versions running in different deployment environments.
/// </summary>
[Route("build-id")]
public class BuildIdController : Controller
{
    private readonly IConfiguration _configuration;

    public BuildIdController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult Index()
    {
        return Json(new { id = _configuration.GetValue<string>("BuildId") });
    }
}
