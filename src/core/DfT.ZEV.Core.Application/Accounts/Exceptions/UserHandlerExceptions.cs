using DfT.ZEV.Common.Exceptions;

namespace DfT.ZEV.Core.Application.Accounts.Exceptions;

internal static class UserHandlerExceptions
{
    public static HandlerException CouldNotCreateUser(string data) => new($"Failed to create user: {data}");
    public static EntityNotFoundException PermissionsNotFound(IEnumerable<Guid> ids) => new("Permissions not found: " + string.Join(", ", ids));
    public static ConflictException UserAlreadyExists(string email) => new($"User with email {email} already exists");
    public static EntityNotFoundException UserNotFound(Guid id) => new($"User with id {id} not found");
}