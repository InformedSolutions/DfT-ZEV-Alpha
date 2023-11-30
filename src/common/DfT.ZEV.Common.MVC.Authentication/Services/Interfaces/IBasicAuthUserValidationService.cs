namespace DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;

/// <summary>
/// Interface for a basic authentication service.
/// </summary>
public interface IBasicAuthUserValidationService
{
    /// <summary>
    /// Checks if provided credentials are valid.
    /// </summary>
    /// <param name="username">Username.</param>
    /// <param name="password">Password.</param>
    /// <returns>True if credentials are valid.</returns>
    public bool AreCredentialsValid(string username, string password);
}
