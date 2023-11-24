// ReSharper disable InconsistentNaming
namespace Zev.Core.Domain.Vehicles.Models;

public class VehicleSummary
{
    public VehicleSummary(string vin)
    {
        Vin = vin;
    }

    public VehicleSummary()
    {
    }

    public string Vin { get; set; } = null!;
    public bool? msv { get; set; }

    public bool? Zev { get; set; }
    public bool? Wca { get; set; }
    public bool? Wcs { get; set; }
    public bool? Rrr { get; set; }

    public bool? ZevApplicable { get; set; }
    public bool? Co2Applicable { get; set; }
    public string? VehicleScheme { get; set; }
    public bool? IncompleteMsv { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
}