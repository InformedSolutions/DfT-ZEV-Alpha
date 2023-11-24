namespace DfT.ZEV.Core.Common.Authentication.Services.Interfaces;

public interface ISignOutService
{
    /// <summary>
    /// Signs user out, removes all identity related cookies.
    /// </summary>
    /// <returns>Task.</returns>
    Task SignOut();
}
