using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.Logging;

public static class LoggerExtensions
{
    public static void LogBusinessEvent(this ILogger logger, string message, params object[] propertyValues)
    {
        using (var scope = logger.BeginScope(new Dictionary<string,object>{["EventType"]= "BusinessEvent"}))
        {
            logger.LogInformation(message,propertyValues);
        }
    }
}