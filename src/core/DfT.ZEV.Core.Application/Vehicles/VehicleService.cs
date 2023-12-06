using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Manufacturers.Models;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using DfT.ZEV.Core.Domain.Vehicles.Services;
using DfT.ZEV.Core.Domain.Vehicles.Values;

namespace DfT.ZEV.Core.Application.Vehicles;

/// <inheritdoc />
public sealed class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;

    private List<string> _manufacturerNames = new();
    private readonly List<Manufacturer> _temporaryManufacturers = new();
    public VehicleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task ApplyRules(IList<Vehicle> vehicles)
    {
        _manufacturerNames = (await _unitOfWork.Manufacturers.GetManufacturerNamesAsync(CancellationToken.None)).ToList();
        foreach (var vehicle in vehicles)
        {
            await ApplyRules(vehicle);
        }
        
        await SaveManufacturers();
    }

    /// <inheritdoc />
    public async Task ApplyRules(Vehicle vehicle)
    {
        ApplyMultistageVan(vehicle);
        ApplyZev(vehicle);
        ApplyFlagsAndApplicability(vehicle);
        await UpsertManufacturer(vehicle);
        //To be implemented later
        //DetermineBonusCredits(vehicle);
    }
    
    /// <inheritdoc />
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

    /// <inheritdoc />
    public Vehicle ApplyZev(Vehicle vehicle)
    {
        const int minRange = 100;

        if (vehicle.Ewltp == 0)
        {
            vehicle.Summary.Wca = false;
            vehicle.Summary.Wcs = false;
            vehicle.Summary.Zev = true;

            if (vehicle.Ber <= minRange) vehicle.Summary.Rrr = false;
        }
        else
        {
            vehicle.Summary.Zev = false;
        }

        return vehicle;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public Vehicle DetermineBonusCredits(Vehicle vehicle)
    {
        throw new NotImplementedException();
    }

    public Task<Vehicle> UpsertManufacturer(Vehicle vehicle)
    {
        if (_manufacturerNames.Contains(vehicle.Mh)) return Task.FromResult(vehicle);
        
        var manufacturer = new Manufacturer(vehicle.Mh).WithCo2Target(0).WithDerogationStatus('N');
        _temporaryManufacturers.Add(manufacturer);
        _manufacturerNames.Add(vehicle.Mh);

        return Task.FromResult(vehicle);
    }

    private async Task SaveManufacturers()
    {
        await _unitOfWork.Manufacturers.BulkInsertAsync(_temporaryManufacturers);
        await _unitOfWork.SaveChangesAsync();
    }
}