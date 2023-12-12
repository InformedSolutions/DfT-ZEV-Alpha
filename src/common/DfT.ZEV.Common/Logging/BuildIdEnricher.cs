using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging;

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

/// <summary>
/// Extension class to allow binding of Serilog enricher.
/// </summary>
public static class BuildIdLoggingExtensions
{
    /// <summary>
    /// Method that can be called when instantiating Serilog.
    /// </summary>
    /// <param name="enrichmentConfiguration">Serilog enricher.</param>
    /// <returns>An enriched Serilog provider instance with properties included to track build ids.</returns>
    public static LoggerConfiguration WithBuildId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        => enrichmentConfiguration.With<BuildIdEnricher>();
    
}