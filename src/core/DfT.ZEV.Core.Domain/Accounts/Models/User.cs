using DfT.ZEV.Core.Domain.Common;
using DfT.ZEV.Core.Domain.Manufacturers.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class User : IAggregateRoot
{
    public Guid Id { get;  }
    public DateTimeOffset CreatedAt { get; }
    
    public ICollection<RolesBridge> RolesBridges { get; private set; } = new List<RolesBridge>();
    public ICollection<ManufacturerPool> ManufacturerPools { get; private set; } = new List<ManufacturerPool>();
    public ICollection<Permission> Permissions { get; private set; } = new List<Permission>();
    public User() { }
    
    public User(Guid id)
    {
        Id = id;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void AddRole(Role role, Manufacturer manufacturer)
    {
        var rolesBridge = new RolesBridge(this, manufacturer, role);
        RolesBridges.Add(rolesBridge);
    }
    
    public void AddPermissions(IEnumerable<Permission> permissions)
    {
        foreach (var permission in permissions)
        {
            Permissions.Add(permission);
        }
    }
}