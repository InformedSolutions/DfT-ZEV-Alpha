using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Zev.Core.Infrastructure.Logging;

/// To-Do: Create Logging strategy for whole solution.
public class SerilogHelper
{
    private static LoggerConfiguration GetBasicSerilogConfig() =>
        new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

    public static ILoggerFactory GetLoggerFactory()
    {
        var loggerFactory = (ILoggerFactory)new LoggerFactory();
        loggerFactory.AddSerilog(GetBasicSerilogConfig().CreateLogger());
        return loggerFactory;
    }
}