namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class ManufacturerPool
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public Guid? PrimaryContactId { get; private set; }
    public User? PrimaryContact { get; private set; }

    protected ManufacturerPool() { }

    public ManufacturerPool(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public ManufacturerPool WithName(string name)
    {
        Name = name;
        return this;
    }
    
    public ManufacturerPool WithPrimaryContact(User primaryContact)
    {
        PrimaryContactId = primaryContact.Id;
        return this;
    }
}