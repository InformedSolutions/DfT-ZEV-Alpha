using System.Net;
using System.Runtime.Serialization;

namespace DfT.ZEV.Common.Exceptions;

/// <summary>
/// Invalid application state exception.
/// Can be thrown when application is in invalid state.
/// For example when something occurred that should never have happened.
/// </summary>
[Serializable]
public class InvalidApplicationStateException : WithHttpStatusBaseException
{
    private const string InvalidApplicationStateExceptionMessage = "Current application state is invalid";

    /// <summary>
    /// Default constructor.
    /// Builds exception with default message and Internal Server Error HTTP status.
    /// </summary>
    public InvalidApplicationStateException()
        : base(InvalidApplicationStateExceptionMessage, HttpStatusCode.InternalServerError)
    {
    }

    /// <summary>
    /// Default constructor.
    /// Builds exception with provided message and Internal Server Error HTTP status.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidApplicationStateException(string message)
        : base($"{InvalidApplicationStateExceptionMessage}: {message}", HttpStatusCode.InternalServerError)
    {
    }

    /// <summary>
    /// Default constructor.
    /// Builds exception with provided message and HTTP status.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="code">HTTP status code.</param>
    public InvalidApplicationStateException(string message, HttpStatusCode code)
        : base($"{InvalidApplicationStateExceptionMessage}: {message}", code)
    {
    }

    /// <summary>
    /// Default constructor.
    /// Builds exception with provided message, inner Exception and HTTP status.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="code">HTTP status code.</param>
    /// <param name="innerException">Inner exception.</param>
    public InvalidApplicationStateException(string message, HttpStatusCode code, Exception innerException)
        : base($"{InvalidApplicationStateExceptionMessage}: {message}", code, innerException)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided message, Internal Server Error HTTP status and inner exception.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public InvalidApplicationStateException(string message, Exception innerException)
        : base(message, HttpStatusCode.InternalServerError, innerException)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided serialization info and context.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected InvalidApplicationStateException(SerializationInfo info, StreamingContext context)
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
