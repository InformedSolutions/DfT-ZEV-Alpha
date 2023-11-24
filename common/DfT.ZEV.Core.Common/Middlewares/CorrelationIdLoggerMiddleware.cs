using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace DfT.ZEV.Common.Middlewares;

/// <summary>
/// A middleware function for logging correlation ID header values.
/// </summary>
public class CorrelationIdLoggerMiddleware
{
    private const string CorrelationIdHeaderKey = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdLoggerMiddleware> _logger;

    public CorrelationIdLoggerMiddleware(RequestDelegate next, ILogger<CorrelationIdLoggerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        string correlationId = null;

        if (httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out StringValues correlationIds))
        {
            correlationId = correlationIds.First(value => !string.IsNullOrEmpty(value)).ToString();
            _logger.LogInformation("Correlation ID from Request Header: {CorrelationId}", correlationId);
        }
        else
        {
            correlationId = Guid.NewGuid().ToString();
            httpContext.Request.Headers.Add(CorrelationIdHeaderKey, correlationId);
            _logger.LogInformation("Generated correlation Id: {CorrelationId}", correlationId);
        }

        httpContext.Response.OnStarting(() =>
        {
            if (!httpContext.Response.Headers.TryGetValue(CorrelationIdHeaderKey, out correlationIds))
            {
                httpContext.Response.Headers.Add(CorrelationIdHeaderKey, correlationId);
            }

            return Task.CompletedTask;
        });

        await _next.Invoke(httpContext);
    }
}
