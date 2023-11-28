namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class Role
{
    public Guid Id { get; }
    public string RoleName { get; private set; }
    
    public ICollection<RolesBridge> RolesBridgeTable { get; private set; } = new List<RolesBridge>();

    public Role() { }

    public Role(string roleName)
    {
        Id = Guid.NewGuid();
        RoleName = roleName;
    }
    
    public Role WithRoleName(string roleName)
    {
        RoleName = roleName;
        return this;
    }
}