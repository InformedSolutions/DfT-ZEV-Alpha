using System.Net;
using System.Runtime.Serialization;

namespace DfT.ZEV.Common.Exceptions;

/// <summary>
/// Entity not found exception.
/// Can be thrown when there is no selected entity in the database
/// or in the configuration file.
/// </summary>
[Serializable]
public class EntityNotFoundException : WithHttpStatusBaseException
{
    private const string InvalidGuidExceptionMessage = "Provided GUID not found in the repository";
    private const string InvalidKeyExceptionMessage = "Provided ID or key not found in the repository";

    /// <summary>
    /// Default constructor.
    /// Builds exception with default message and Not Found HTTP status.
    /// </summary>
    public EntityNotFoundException()
        : base(InvalidGuidExceptionMessage, HttpStatusCode.NotFound)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided message and Not Found HTTP status.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public EntityNotFoundException(string message)
        : base(message, HttpStatusCode.NotFound)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with default message built with the use of
    /// passed entity name and ID.
    /// Default Not Found HTTP status is used.
    /// </summary>
    /// <param name="entityName">Entity name.</param>
    /// <param name="entityId">Entity ID.</param>
    public EntityNotFoundException(string entityName, Guid entityId)
        : this(entityName, entityId, HttpStatusCode.NotFound, null)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with default message built with the use of
    /// passed entity name and string ID/key.
    /// Default Not Found HTTP status is used.
    /// </summary>
    /// <param name="entityName">Entity name.</param>
    /// <param name="entityId">Entity ID/key.</param>
    public EntityNotFoundException(string entityName, string entityId)
        : base($"{InvalidKeyExceptionMessage}: {entityName} ({entityId})", HttpStatusCode.NotFound, null)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with default message built with the use of
    /// passed entity name and ID.
    /// Besides that any HTTP status code or inner exception
    /// can be passed.
    /// </summary>
    /// <param name="entityName">Entity name.</param>
    /// <param name="entityId">Entity ID.</param>
    /// <param name="code">HTTP status code.</param>
    /// <param name="innerException">Inner exception.</param>
    public EntityNotFoundException(string entityName, Guid entityId, HttpStatusCode code, Exception innerException)
        : base($"{InvalidGuidExceptionMessage}: {entityName} ({entityId})", code, innerException)
    {
    }

    /// <summary>
    /// Constructor.
    /// Builds exception with provided serialization info and context.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
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
