using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging.Enrichers;

/// <summary>
/// Serilog provider extension to log compilation settings of assemblies.
/// </summary>
public class BuildIdEnricher : ILogEventEnricher
{
    private readonly string _buildIdEnvironmentVariable;

    public BuildIdEnricher()
    {
        _buildIdEnvironmentVariable = Environment.GetEnvironmentVariable("BUILDID");
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (!string.IsNullOrEmpty(_buildIdEnvironmentVariable))
        {
            var property = propertyFactory.CreateProperty("BuildId", _buildIdEnvironmentVariable);
            logEvent.AddOrUpdateProperty(property);
        }
    }
}