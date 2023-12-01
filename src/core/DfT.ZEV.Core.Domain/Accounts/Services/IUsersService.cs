using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Services;

public interface IUsersService
{
    public Task UpdateUserClaimsAsync(User user);
}