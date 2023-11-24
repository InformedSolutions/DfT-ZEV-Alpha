using DfT.ZEV.Core.Common.Authentication.ViewModels;

namespace DfT.ZEV.Core.Common.Authentication.Services.Interfaces;

public interface IForgottenPasswordService
{
    /// <summary>
    /// Sends email with token and saves verification code in user pool.
    /// </summary>
    /// <param name="viewModel">RequestPasswordChangeViewModel viewmodel.</param>
    /// <returns>AuthResult.</returns>
    Task<AuthResult> RequestForgottenPasswordChange(RequestPasswordChangeViewModel viewModel);

    /// <summary>
    /// Verifies forgotten password token.
    /// </summary>
    /// <param name="token">Token to be verified.</param>
    /// <returns>AuthResult.Success if token is valid.</returns>
    Task<AuthResult> VerifyForgottenPasswordToken(string token);

    /// <summary>
    /// Verifies forgotten password token and changes user password.
    /// </summary>
    /// <param name="viewModel">ForgottenPasswordChangeViewModel view model with token and new password.</param>
    /// <returns>AuthResult.</returns>
    Task<AuthResult> ChangeForgottenPassword(ForgottenPasswordChangeViewModel viewModel);
}
