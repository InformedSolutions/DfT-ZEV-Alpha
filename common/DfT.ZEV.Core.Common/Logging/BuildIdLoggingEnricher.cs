using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Serilog provider extension to log compilation settings of assemblies.
/// </summary>
public class BuildIdLoggingEnricher : ILogEventEnricher
{
    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var buildIdEnvironmentVariable = Environment.GetEnvironmentVariable("BUILDID");

        if (!string.IsNullOrEmpty(buildIdEnvironmentVariable))
        {
            var property = propertyFactory.CreateProperty("BuildId", buildIdEnvironmentVariable);
            logEvent.AddOrUpdateProperty(property);
        }
    }
}
