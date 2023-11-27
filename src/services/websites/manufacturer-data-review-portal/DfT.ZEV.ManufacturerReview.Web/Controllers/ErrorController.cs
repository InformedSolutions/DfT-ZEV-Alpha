using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.ManufacturerReview.Web.Controllers;

/// <summary>
/// Custom error controller. Please see ExceptionMiddleware.cs for bindings to this controller from general
/// exceptions.
/// </summary>
[Route("error")]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="logger">Logging provider implementation.</param>
    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 400 bad request handler.
    /// </summary>
    /// <returns>Customised error page.</returns>
    [Route("400")]
    public IActionResult InvalidRequest()
    {
        return View("Error");
    }

    /// <summary>
    /// 404 not found handler.
    /// </summary>
    /// <returns>Customised 404 not found page.</returns>
    [Route("404")]
    public IActionResult PageNotFound()
    {
        return View();
    }

    /// <summary>
    /// 500 error handler.
    /// </summary>
    /// <returns>Customised 500 service error page.</returns>
    [Route("500")]
    public IActionResult Error()
    {
        return View();
    }
}
