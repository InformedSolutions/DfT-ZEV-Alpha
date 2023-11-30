namespace DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;

public class ManufacturyPermissions
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
public class ManufacturiesWithPermissions
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public IEnumerable<ManufacturyPermissions> Permissions { get; set; } = null!;
}

public class GetAllUsersDTO
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public IEnumerable<ManufacturiesWithPermissions> Manufacturies { get; set; } = null!;
}