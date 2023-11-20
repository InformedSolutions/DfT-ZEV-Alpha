namespace Zev.Core.Domain.Vehicles.Services;

public interface IVehicleService
{
    public IEnumerable<Vehicle> ApplyRules(IList<Vehicle> vehicles);
    
    public Vehicle ApplyMultistageVan(Vehicle vehicle);
    public Vehicle ApplyZev(Vehicle vehicle);
    public Vehicle ApplyFlagsAndApplicability(Vehicle vehicle);
    public Vehicle DetermineBonusCredits(Vehicle vehicle);
}