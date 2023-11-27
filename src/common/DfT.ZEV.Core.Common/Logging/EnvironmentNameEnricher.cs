using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Serilog provider extension to log settings of runtime.
/// </summary>
public class EnvironmentNameEnricher : ILogEventEnricher
{
    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var environmentNameVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (!string.IsNullOrEmpty(environmentNameVariable))
        {
            var property = propertyFactory.CreateProperty("AspNetCoreEnvironment", environmentNameVariable);
            logEvent.AddOrUpdateProperty(property);
        }
    }
}