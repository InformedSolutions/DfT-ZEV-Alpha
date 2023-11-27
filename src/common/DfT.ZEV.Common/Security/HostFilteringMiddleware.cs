using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DfT.ZEV.Common.Security;

/// <summary>
/// Host header filtering middleware class.
/// Please note this is necessary over the default AllowedHosts configuration
/// capabilities in .NET Core, to allow specific path exclusions and presenting a bespoke error page.
/// </summary>
public class HostFilteringMiddleware
{
    private readonly RequestDelegate _next;

    private readonly HostFilteringOptions _options;

    /// <summary>
    /// Default constuctor.
    /// </summary>
    /// <param name="next">A function that can process an HTTP request.</param>
    /// <param name="options">Configured middleware options for host filtering.</param>
    public HostFilteringMiddleware(RequestDelegate next, HostFilteringOptions options)
    {
        _next = next;
        _options = options;
    }

    /// <summary>
    /// Custom implementation of middleware component for allowed host filtering.
    /// </summary>
    /// <param name="httpContext">Encapsulates all HTTP-specific information about an individual HTTP request.</param>
    /// <returns>Task.</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Allow wildcard support
        if (_options.AllowedHostnames.Contains("*"))
        {
            await _next(httpContext);
            return;
        }

        if (!_options.AllowedHostnames.Contains(httpContext.Request.Host.Host) && !_options.ExcludedPaths.Contains(httpContext.Request.Path))
        {
            httpContext.Response.StatusCode = 400;
            httpContext.Request.Path = _options.ErrorRoute;
        }

        await _next(httpContext);
    }
}
