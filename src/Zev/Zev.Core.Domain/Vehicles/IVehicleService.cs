namespace Zev.Core.Domain.Vehicles.Services;

/// <summary>
/// Represents a service for managing vehicles.
/// </summary>
public interface IVehicleService
{
    /// <summary>
    /// Applies the rules to a list of vehicles.
    /// </summary>
    /// <param name="vehicles">The list of vehicles to apply the rules to.</param>
    void ApplyRules(IList<Vehicle> vehicles);

    /// <summary>
    /// Applies the rules to a single vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to apply the rules to.</param>
    void ApplyRules(Vehicle vehicle);

    /// <summary>
    /// Applies the multistage van rules to a vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to apply the multistage van rules to.</param>
    /// <returns>The vehicle with the multistage van rules applied.</returns>
    Vehicle ApplyMultistageVan(Vehicle vehicle);

    /// <summary>
    /// Applies the ZEV (Zero Emission Vehicle) rules to a vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to apply the ZEV rules to.</param>
    /// <returns>The vehicle with the ZEV rules applied.</returns>
    Vehicle ApplyZev(Vehicle vehicle);

    /// <summary>
    /// Applies the flags and applicability rules to a vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to apply the flags and applicability rules to.</param>
    /// <returns>The vehicle with the flags and applicability rules applied.</returns>
    Vehicle ApplyFlagsAndApplicability(Vehicle vehicle);

    /// <summary>
    /// Determines the bonus credits for a vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to determine the bonus credits for.</param>
    /// <returns>The vehicle with the bonus credits determined.</returns>
    Vehicle DetermineBonusCredits(Vehicle vehicle);
}