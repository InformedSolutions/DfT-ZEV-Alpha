namespace Zev.Core.Domain.Vehicles.Services;

public interface IVehicleService
{
    public void ApplyRules(IList<Vehicle> vehicles);
    public void ApplyRules(Vehicle vehicle);
    
    public Vehicle ApplyMultistageVan(Vehicle vehicle);
    public Vehicle ApplyZev(Vehicle vehicle);
    public Vehicle ApplyFlagsAndApplicability(Vehicle vehicle);
    public Vehicle DetermineBonusCredits(Vehicle vehicle);
}