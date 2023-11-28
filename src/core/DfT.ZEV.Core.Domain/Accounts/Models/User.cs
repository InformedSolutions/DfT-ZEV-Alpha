using DfT.ZEV.Core.Domain.Common;

namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class User : IAggregateRoot
{
    public Guid Id { get;  }
    public DateTimeOffset CreatedAt { get; }
    
    public ICollection<RolesBridge> RolesBridges { get; private set; } = new List<RolesBridge>();
    public ICollection<ManufacturerPool> ManufacturerPools { get; private set; } = new List<ManufacturerPool>();

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
}