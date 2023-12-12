using Serilog;
using Serilog.Configuration;

namespace DfT.ZEV.Common.Logging.Enrichers;

/// <summary>
/// Extension class to allow binding of Serilog enricher.
/// </summary>
public static class EnrichersExtensions
{
    /// <summary>
    /// Method that can be called when instantiating Serilog.
    /// </summary>
    /// <param name="enrichmentConfiguration">Serilog enricher.</param>
    /// <returns>An enriched Serilog provider instance with properties included to track build ids.</returns>
    public static LoggerConfiguration WithBuildId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        => enrichmentConfiguration.With<BuildIdEnricher>();
    
}