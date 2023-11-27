using DfT.ZEV.Common.ConstantKeys;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Serilog provider extension to log correlation IDs from request headers.
/// </summary>
public class HttpContextDerivedCorrelationIdEnricher : ILogEventEnricher
{
    private const string CorrelationIdHeaderKey = "X-Correlation-Id";
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <returns>Enricher instance to log correlation IDs.</returns>
    public HttpContextDerivedCorrelationIdEnricher()
        : this(new HttpContextAccessor())
    {
    }

    /// <summary>
    /// Constructor overload.
    /// </summary>
    /// <param name="contextAccessor">An HTTP Context accessor instance.</param>
    public HttpContextDerivedCorrelationIdEnricher(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (_contextAccessor.HttpContext != null
            && _contextAccessor.HttpContext.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out StringValues correlationIds))
        {
            var correlationId = correlationIds.FirstOrDefault(value => !string.IsNullOrEmpty(value))?.ToString();

            if (correlationId != null)
            {
                var correlationIdProperty = propertyFactory.CreateProperty(RequestIdPropertyNames.CorrelationId, correlationId);
                logEvent.AddOrUpdateProperty(correlationIdProperty);
            }
        }
    }
}
