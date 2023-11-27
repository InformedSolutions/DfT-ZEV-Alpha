using System.Net;
using System.Runtime.Serialization;

namespace DfT.ZEV.Common.Exceptions;

/// <summary>
/// Network issue exception.
/// Can be thrown when application has some connection issues.
/// For example when connection issue occurs during API call that saves the data.
/// </summary>
[Serializable]
public class NetworkIssuesException : WithHttpStatusBaseException
{
    private const string InternalNetworkIssuesDetected = "Internal network issues detected";

    /// <summary>
    /// Default constructor.
    /// Builds exception with default message and Internal Server Error HTTP status.
    /// </summary>
    public NetworkIssuesException()
        : base(InternalNetworkIssuesDetected, HttpStatusCode.InternalServerError)
    {
    }

    /// <summary>
    /// Default constructor.
    /// Builds exception with default message and provided HTTP status code.
    /// </summary>
    /// <param name="code">HTTP status code.</param>
    public NetworkIssuesException(HttpStatusCode code)
        : base(InternalNetworkIssuesDetected, code)
    {
    }

    /// <summary>
    /// Default constructor.
    /// Builds exception with default message, provided HTTP status code and inner exception.
    /// </summary>
    /// <param name="code">HTTP status code.</param>
    /// <param name="innerException">Inner exception.</param>
    public NetworkIssuesException(HttpStatusCode code, Exception innerException)
        : base(InternalNetworkIssuesDetected, code, innerException)
    {
    }

    /// <summary>
    /// Default constructor.
    /// Builds exception with default message, Internal Server Error HTTP status and inner exception.
    /// </summary>
    /// <param name="innerException">Inner exception.</param>
    public NetworkIssuesException(Exception innerException)
        : base(InternalNetworkIssuesDetected, HttpStatusCode.InternalServerError, innerException)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided serialization info and context.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected NetworkIssuesException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (<see cref="StreamingContext" />) for this serialization.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}
