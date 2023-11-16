namespace Zev.Services.ComplianceCalculationService.Tests;

public class Helpers
{
    /// <summary>
    /// Reads the contents of the "TestFiles/vehicles.csv" file and returns it as a string.
    /// </summary>
    /// <returns>The contents of the "TestFiles/vehicles.csv" file as a string.</returns>
    public static string GetTestVehiclesFromFile()
    {
        return File.ReadAllText("TestFiles/vehicles.csv");
    }
}