using Microsoft.Extensions.Configuration;
using Serilog;

namespace DfT.ZEV.Common.Logging;

public class BusinessEventLogger : IBusinessEventLogger
{
    private readonly ILogger _logger;

    public BusinessEventLogger(IConfiguration configuration)
    {
        _logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
            .Enrich.WithCorrelationIdLogging()
            .Enrich.WithAssemblyCompilationModeLogging()
            .Enrich.WithBuildIdLogging()
            .Enrich.WithEnvironmentNameLogging()
            .Enrich.WithBusinessLogging()
            .Enrich.WithSessionIdLogging()
            .CreateLogger()
            .ForContext<BusinessEventLogger>();
    }

    public void LogBusiness(string message)
    {
        _logger.Information(message);
    }

    public void LogBusiness(string message, params object[] propertyValues)
    {
        _logger.Information(message, propertyValues);
    }
}
