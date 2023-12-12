using DfT.ZEV.Common.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.Middlewares;

/// <summary>
/// A middleware function for logging correlation ID header values.
/// </summary>
public class PageViewLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly bool _enableAuditLogging;

    public PageViewLoggerMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));

        _enableAuditLogging = configuration.GetValue<bool>("EnableAuditLogging");
    }

    public async Task Invoke(HttpContext httpContext, ILogger<PageViewLoggerMiddleware> logger)
    {
        if (_enableAuditLogging && httpContext.Request.Method == "GET")
        {
            logger.LogBusinessEvent("Page viewed: {path}", httpContext.Request.Path);
        }

        await _next.Invoke(httpContext);
    }
}