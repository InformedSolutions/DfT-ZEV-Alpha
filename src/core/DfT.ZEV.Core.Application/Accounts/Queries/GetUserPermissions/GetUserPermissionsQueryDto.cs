namespace DfT.ZEV.Core.Application.Accounts.Queries.GetUserPermissions;

public class ManufacturerPermission
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
}

public class GetUserPermissionsQueryDto
{
  public Guid UserId { get; set; }
  public Guid ManufacturerId { get; set; }
  public string ManufacturerName { get; set; } = null!;
  public IEnumerable<ManufacturerPermission> Permissions { get; set; } = null!;
}
