using DfT.ZEV.Common.Exceptions;

namespace DfT.ZEV.Core.Application.Accounts.Exceptions;

internal static class UserHandlerExceptions
{
    public static HandlerException CouldNotCreateUser(string data) => new($"Failed to create user: {data}");
    
}