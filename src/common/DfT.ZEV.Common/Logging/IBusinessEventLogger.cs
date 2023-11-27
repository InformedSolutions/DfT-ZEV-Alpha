namespace DfT.ZEV.Common.Logging;

/// <summary>
/// Logging service for capturing business events.
/// </summary>
public interface IBusinessEventLogger
{
    void LogBusiness(string message);

    void LogBusiness(string message, params object[] propertyValues);
}
