using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Serilog provider extension to log business type logs.
/// </summary>
public class BusinessLoggingEnricher : ILogEventEnricher
{
    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var property = propertyFactory.CreateProperty("BusinessEvent", true);
        logEvent.AddOrUpdateProperty(property);
    }
}
