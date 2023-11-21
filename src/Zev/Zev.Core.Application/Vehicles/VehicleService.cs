using Zev.Core.Domain.Vehicles;
using Zev.Core.Domain.Vehicles.Services;

namespace Zev.Core.Application.Vehicles;

public class VehicleService : IVehicleService
{
    public void ApplyRules(IList<Vehicle> vehicles)
    {
        foreach (var vehicle in vehicles)
        {
            ApplyMultistageVan(vehicle);
            ApplyZev(vehicle);
            ApplyFlagsAndApplicability(vehicle);
            
            //To be implemented later
            //DetermineBonusCredits(vehicle);
        }

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
        const int minRange = 100;
        
        if (vehicle.Ewltp == 0)
        {
            vehicle.Summary.Wca = false;
            vehicle.Summary.Wcs = false;

            if (vehicle.Ber >= minRange)
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
            if (vehicle.TAN == VehicleTan.M1)
            {
                vehicle.Summary.ZevApplicable = true;
                vehicle.Summary.Co2Applicable = true;
                vehicle.Summary.VehicleScheme = VehicleScheme.Car;
            }else if (vehicle.TAN == VehicleTan.N1)
            {
                vehicle.Summary.ZevApplicable = true;
                vehicle.Summary.Co2Applicable = true;
                vehicle.Summary.VehicleScheme = VehicleScheme.Van;
            }
            else
            {
                vehicle.Summary.Co2Applicable = false;
                if (vehicle is { TAN: VehicleTan.N2, Summary.Zev: true, TPMLM: < 4250 })
                {
                    vehicle.Summary.ZevApplicable = true;
                    vehicle.Summary.VehicleScheme = VehicleScheme.Van;
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