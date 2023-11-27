using DfT.ZEV.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.Middlewares;

/// <summary>
/// This middleware will catch unhandled errors
/// log exception and throw.
/// </summary>
public class WebsiteExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;

    public WebsiteExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var logger = _loggerFactory.CreateLogger<WebsiteExceptionMiddleware>();

        try
        {
            await _next(httpContext);
        }
        catch (EntityNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong, error: {ExceptionMessage}", ex.Message);
            throw;
        }
    }
}
