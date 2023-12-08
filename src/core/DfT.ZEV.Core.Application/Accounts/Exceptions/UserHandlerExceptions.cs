using DfT.ZEV.Common.Exceptions;

namespace DfT.ZEV.Core.Application.Accounts.Exceptions;

/// <summary>
/// Provides methods for creating exceptions related to user handlers.
/// </summary>
internal static class UserHandlerExceptions
{
    /// <summary>
    /// Creates a new handler exception indicating that a user could not be created.
    /// </summary>
    /// <param name="data">The data associated with the exception.</param>
    /// <returns>A new handler exception.</returns>
    public static HandlerException CouldNotCreateUser(string data) => new($"Failed to create user: {data}");

    /// <summary>
    /// Creates a new entity not found exception indicating that permissions were not found.
    /// </summary>
    /// <param name="ids">The identifiers of the permissions that were not found.</param>
    /// <returns>A new entity not found exception.</returns>
    public static EntityNotFoundException PermissionsNotFound(IEnumerable<Guid> ids) => new("Permissions not found: " + string.Join(", ", ids));

    /// <summary>
    /// Creates a new conflict exception indicating that a user already exists.
    /// </summary>
    /// <param name="email">The email of the user that already exists.</param>
    /// <returns>A new conflict exception.</returns>
    public static ConflictException UserAlreadyExists(string email) => new($"User with email {email} already exists");

    /// <summary>
    /// Creates a new entity not found exception indicating that a user was not found.
    /// </summary>
    /// <param name="id">The identifier of the user that was not found.</param>
    /// <returns>A new entity not found exception.</returns>
    public static EntityNotFoundException UserNotFound(Guid id) => new($"User with id {id} not found");
}