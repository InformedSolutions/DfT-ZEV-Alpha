using Serilog;
using Serilog.Configuration;

namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Extension class to allow binding of Serilog enricher.
/// </summary>
public static class BusinessLoggingExtensions
{
    /// <summary>
    /// Method that can be called when instantiating Serilog.
    /// </summary>
    /// <param name="enrichmentConfiguration">Serilog enricher.</param>
    /// <returns>An enriched Serilog provider instance with properties included to track business logs.</returns>
    public static LoggerConfiguration WithBusinessLogging(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null)
        {
            throw new ArgumentNullException(nameof(enrichmentConfiguration));
        }

        return enrichmentConfiguration.With<BusinessLoggingEnricher>();
    }
}
