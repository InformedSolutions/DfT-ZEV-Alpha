using Serilog.Core;
using Serilog.Events;

namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Serilog provider extension to log compilation settings of assemblies.
/// </summary>
public class AssemblyCompilationLoggingEnricher : ILogEventEnricher
{
    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        string compilationPropertyName = "AssemblyBuildMode";

#if DEBUG
        var property = propertyFactory.CreateProperty(compilationPropertyName, "Debug");
        logEvent.AddOrUpdateProperty(property);
#else
        var property = propertyFactory.CreateProperty(compilationPropertyName, "Release");
        logEvent.AddOrUpdateProperty(property);
#endif
    }
}
