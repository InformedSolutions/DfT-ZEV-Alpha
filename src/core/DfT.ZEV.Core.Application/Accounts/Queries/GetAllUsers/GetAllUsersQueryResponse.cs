namespace DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;

public class GetAllUsersQueryResponse
{
    public IEnumerable<GetAllUsersDto> Users { get; set; } = null!;
}