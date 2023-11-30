namespace DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;

public class UserPermissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public class GetAllUsersDTO
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public IEnumerable<UserPermissionDto> Permissions { get; set; } = null!;
}