using System.Net;
using System.Runtime.Serialization;

namespace DfT.ZEV.Common.Exceptions;

/// <summary>
/// With HTTP status base exception.
/// Can be derived or thrown when HTTP status code is required.
/// </summary>
[Serializable]
public class WithHttpStatusBaseException : Exception
{
    private const string DefaultExceptionMessage = "Unknown Application Exception occurred.";

    /// <summary>
    /// Gets or sets HTTP status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Default constructor.
    /// Builds exception with default message and Internal Server Error HTTP status.
    /// </summary>
    public WithHttpStatusBaseException()
        : this(DefaultExceptionMessage)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided message and Internal Server Error HTTP status.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public WithHttpStatusBaseException(string message)
        : this(message, HttpStatusCode.InternalServerError)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided message and HTTP status code.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="code">HTTP status code.</param>
    public WithHttpStatusBaseException(string message, HttpStatusCode code)
        : this(message, code, null)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided message, HTTP status code and inner exception.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="code">HTTP status code.</param>
    /// <param name="innerException">Inner exception.</param>
    public WithHttpStatusBaseException(string message, HttpStatusCode code, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = code;
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided serialization info and context.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected WithHttpStatusBaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        StatusCode = (HttpStatusCode)info.GetValue(nameof(StatusCode), typeof(HttpStatusCode));
    }

    /// <summary>
    /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (<see cref="StreamingContext" />) for this serialization.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(StatusCode), StatusCode);
        base.GetObjectData(info, context);
    }
}
