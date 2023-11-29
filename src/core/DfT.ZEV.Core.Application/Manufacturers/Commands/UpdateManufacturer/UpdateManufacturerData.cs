namespace DfT.ZEV.Core.Application.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerData
{
    public string? Name { get; set; }
    public Guid? PoolMemberId { get; set; }
    public float? Co2Target { get; set; }
    public char? DerogationStatus { get; set; }
}