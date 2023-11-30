namespace DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;

public class GetAllUsersQueryResponse
{
    public IEnumerable<GetAllUsersDTO> Users { get; set; }
}