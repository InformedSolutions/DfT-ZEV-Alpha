namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class Permission
{
    public Guid Id { get; private set; }
    public string PermissionName { get; private set; } = null!;
    
    public ICollection<UserManufacturerBridge> UserManufacturerBridges { get; private set; } = new List<UserManufacturerBridge>();
    
    protected Permission() { }
    
    public Permission(string permissionName)
    {
        Id = Guid.NewGuid();
        PermissionName = permissionName;
    }
}