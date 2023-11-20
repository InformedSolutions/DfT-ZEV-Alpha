using Zev.Core.Domain.Vehicles;
using Zev.Core.Domain.Vehicles.Services;

namespace Zev.Core.Application.Vehicles;

public class VehicleService : IVehicleService
{
    public IEnumerable<Vehicle> ApplyRules(IList<Vehicle> vehicles)
    {
        foreach (var vehicle in vehicles)
        {
            ApplyMultistageVan(vehicle);
            ApplyZev(vehicle);
            ApplyFlagsAndApplicability(vehicle);
            DetermineBonusCredits(vehicle);
        }

        return vehicles;
    }

    public Vehicle ApplyMultistageVan(Vehicle vehicle)
    {
        if (vehicle.MM is null && vehicle.MRVL is null)
        {
            vehicle.Summary.msv = false;
        }
        else
        {
            vehicle.Summary.msv = true;
            vehicle.Summary.IncompleteMsv = true;
        }

        return vehicle;
    }

    public Vehicle ApplyZev(Vehicle vehicle)
    {
        if (vehicle.Ewltp == 0)
        {
            vehicle.Summary.Wca = false;
            vehicle.Summary.Wcs = false;

            if (vehicle.Ber >= 100)
            {
                vehicle.Summary.Zev = true;
            }
            else
            {
                vehicle.Summary.Zev = true;
                vehicle.Summary.Rrr = false;
            }
        }
        else
        {
            vehicle.Summary.Zev = false;
        }

        return vehicle;
    }

    public Vehicle ApplyFlagsAndApplicability(Vehicle vehicle)
    {
        if (vehicle.Spvc is null)
        {
            if (vehicle.TAN == "M1")
            {
                vehicle.Summary.ZevApplicable = true;
                vehicle.Summary.Co2Applicable = true;
                vehicle.Summary.VehicleScheme = "car";
            }else if (vehicle.TAN == "N1")
            {
                vehicle.Summary.ZevApplicable = true;
                vehicle.Summary.Co2Applicable = true;
                vehicle.Summary.VehicleScheme = "van";
            }
            else
            {
                vehicle.Summary.Co2Applicable = false;
                if (vehicle is { TAN: "N2", Summary.Zev: true, TPMLM: < 4250 })
                {
                    vehicle.Summary.ZevApplicable = true;
                    vehicle.Summary.VehicleScheme = "van";
                }
                else
                {
                    vehicle.Summary.ZevApplicable = false;
                }
            }
        }
        else
        {
            vehicle.Summary.Co2Applicable = false;
            vehicle.Summary.ZevApplicable = false;
        }

        return vehicle;
    }

    public Vehicle DetermineBonusCredits(Vehicle vehicle)
    {
        throw new NotImplementedException();
    }
}