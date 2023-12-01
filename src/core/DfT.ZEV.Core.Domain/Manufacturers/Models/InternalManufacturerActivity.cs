// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace DfT.ZEV.Core.Domain.Manufacturers.Models;

public sealed class InternalManufacturerActivity
{
    public Guid Id { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public string Status { get; private set; } = null!;
    public string ActivityType { get; private set; } = null!;
    public DateTimeOffset ActionInitiated { get; private set; }

    private InternalManufacturerActivity() { }
    
    public InternalManufacturerActivity(Guid manufacturerId, string status, string activityType, DateTimeOffset actionInitiated)
    {
        Id = Guid.NewGuid();
        ManufacturerId = manufacturerId;
        Status = status;
        ActivityType = activityType;
        ActionInitiated = actionInitiated;
    }
}