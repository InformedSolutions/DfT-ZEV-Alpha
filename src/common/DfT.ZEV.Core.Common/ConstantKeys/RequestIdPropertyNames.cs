namespace DfT.ZEV.Common.ConstantKeys;

/// <summary>
/// Constant keys for log property names used to track request and trace IDs.
/// </summary>
public static class RequestIdPropertyNames
{
    /// <summary>
    /// An ID used to correlate log entries from multiple different services together.
    /// </summary>
    public const string CorrelationId = "CorrelationId";

    /// <summary>
    /// The ID of a single request.
    /// </summary>
    public const string RequestId = "RequestId";

    /// <summary>
    /// The ID of the session.
    /// </summary>
    public const string SessionId = "SessionId";
}
