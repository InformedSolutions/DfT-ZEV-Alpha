using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Domain.Manufacturers.Models;

public sealed class Manufacturer
{
    public Guid Id { get; }
    public string Name { get; private set; } = null!;
    public Guid PoolMemberId { get; private set; }
    public float Co2Target { get; private set; }
    public char DerogationStatus { get; private set; }
    
    public ICollection<UserManufacturerBridge> UserBridges { get; private set; } = new List<UserManufacturerBridge>();
    private Manufacturer() { }

    public Manufacturer(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public Manufacturer WithName(string name)
    {
        Name = name;
        return this;
    }
    
    public Manufacturer WithPoolMemberId(Guid poolMemberId)
    {
        PoolMemberId = poolMemberId;
        return this;
    }
    
    public Manufacturer WithCo2Target(float co2Target)
    {
        Co2Target = co2Target;
        return this;
    }
    
    public Manufacturer WithDerogationStatus(char derogationStatus)
    {
        DerogationStatus = derogationStatus;
        return this;
    }
}