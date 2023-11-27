using DfT.ZEV.Common.MVC.Authentication.ViewModels;

namespace DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;

public interface ISignInService
{
    /// <summary>
    /// Signs in user. Checks all restrictions eg. lockout, email not verified. Saves failed login attempt.
    /// </summary>
    /// <param name="viewModel">SignInViewModel viewmodel.</param>
    /// <returns>AuthResult.</returns>
    Task<AuthResult> SignIn(SignInViewModel viewModel);

    /// <summary>
    /// Returns common setting for authentication system.
    /// Must be true to use sliding access expiration.
    /// </summary>
    /// <returns>True if login should be persistent.</returns>
    bool IsPersistent();
}
