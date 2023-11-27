using DfT.ZEV.Common.ConstantKeys;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Serilog provider extension to log session id.
/// </summary>
public class SessionIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <returns>Enricher instance to log session id.</returns>
    public SessionIdEnricher()
        : this(new HttpContextAccessor())
    {
    }

    /// <summary>
    /// Constructor overload.
    /// </summary>
    /// <param name="contextAccessor">An HTTP Context accessor instance.</param>
    public SessionIdEnricher(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (_contextAccessor.HttpContext != null
            && _contextAccessor.HttpContext.Request.Path.HasValue
            && _contextAccessor.HttpContext.Request.Path.Value.StartsWith("/sessions/"))
        {
            var path = _contextAccessor.HttpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (path.Length < 2)
            {
                return;
            }

            var sessionId = path[1];

            if (!string.IsNullOrEmpty(sessionId))
            {
                var sessionIdProperty = propertyFactory.CreateProperty(RequestIdPropertyNames.SessionId, sessionId);
                logEvent.AddOrUpdateProperty(sessionIdProperty);
            }
        }
    }
}
