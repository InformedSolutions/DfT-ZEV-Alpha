using Zev.Core.Domain.Vehicles;
using Zev.Core.Domain.Vehicles.Models;
using Zev.Core.Domain.Vehicles.Services;
using Zev.Core.Domain.Vehicles.Values;

namespace Zev.Core.Application.Vehicles;

/// <inheritdoc/>
public class VehicleService : IVehicleService
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void ApplyRules(Vehicle vehicle)
    {
        ApplyMultistageVan(vehicle);
        ApplyZev(vehicle);
        ApplyFlagsAndApplicability(vehicle);
    }

    /// <inheritdoc/>
    public Vehicle ApplyMultistageVan(Vehicle vehicle)
    {
        if (vehicle.MM is null && vehicle.MRVL is null)
        {
            vehicle.Summary.msv = false;
            vehicle.Summary.IncompleteMsv = false;
        }
        else
        {
            vehicle.Summary.msv = true;
            vehicle.Summary.IncompleteMsv = true;
        }

        return vehicle;
    }

    /// <inheritdoc/>
    public Vehicle ApplyZev(Vehicle vehicle)
    {
        const int minRange = 100;

        if (vehicle.Ewltp == 0)
        {
            vehicle.Summary.Wca = false;
            vehicle.Summary.Wcs = false;
            vehicle.Summary.Zev = true;

            if (vehicle.Ber <= minRange)
            {
                vehicle.Summary.Rrr = false;
            }
        }
        else
        {
            vehicle.Summary.Zev = false;
        }

        return vehicle;
    }

    /// <inheritdoc/>
    public Vehicle ApplyFlagsAndApplicability(Vehicle vehicle)
    {
        if (string.IsNullOrWhiteSpace(vehicle.Spvc))
        {
            if (vehicle.Ct == VehicleTan.M1)
            {
                vehicle.Summary.ZevApplicable = true;
                vehicle.Summary.Co2Applicable = true;
                vehicle.Summary.VehicleScheme = VehicleScheme.Car;
            }
            else if (vehicle.Ct == VehicleTan.N1)
            {
                vehicle.Summary.ZevApplicable = true;
                vehicle.Summary.Co2Applicable = true;
                vehicle.Summary.VehicleScheme = VehicleScheme.Van;
            }
            else
            {
                vehicle.Summary.Co2Applicable = false;
                if (vehicle is { Ct: VehicleTan.N2, Summary.Zev: true, TPMLM: < 4250 })
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

    /// <inheritdoc/>
    public Vehicle DetermineBonusCredits(Vehicle vehicle)
    {
        throw new NotImplementedException();
    }
}