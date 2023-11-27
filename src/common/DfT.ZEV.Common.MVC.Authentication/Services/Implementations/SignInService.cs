using DfT.ZEV.Common.Logging;
using DfT.ZEV.Common.MVC.Authentication;
using DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;
using DfT.ZEV.Common.MVC.Authentication.ViewModels;
using Microsoft.Extensions.Logging;

namespace Informed.Common.Auth.Areas.Auth.Services;

public class SignInService : ISignInService
{
    private const bool _isPersistent = true;

    private readonly ILogger<SignInService> _logger;
    private readonly IBusinessEventLogger _businessEventLogger;

    public SignInService(
        ILogger<SignInService> logger,
        IBusinessEventLogger businessEventLogger)
    {
        _logger = logger;
        _businessEventLogger = businessEventLogger;
    }

    public bool IsPersistent() => _isPersistent;

    public async Task<AuthResult> SignIn(SignInViewModel viewModel)
    {
        if (!string.IsNullOrEmpty(viewModel.UserId))
        {
            // UserId works as honeypot captcha. If filled, there may have been hack trial.
            _businessEventLogger.LogBusiness("Honeypot captcha has been filled.");
            return AuthResult.Failed(AuthErrorMessages.SignInErrorMessage);
        }

        // var user = await _userManager.FindByEmailAsync(viewModel.Email);

        // if (user == null)
        // {
        //     _logger.LogInformation("Not existing user sign in attempt");
        //     return AuthResult.Failed(AuthErrorMessages.SignInErrorMessage);
        // }

        // if (!IsEmailVerified(user))
        // {
        //     _logger.LogInformation("Unverified user sign in attempt");
        //     return AuthResult.Failed(AuthErrorMessages.SignInErrorMessage);
        // }

        // if (await IsAccountLocked(user))
        // {
        //     _logger.LogInformation("Locked user sign in attempt");
        //     return AuthResult.Failed(AuthErrorMessages.SignInErrorMessage);
        // }

        // if (HasPasswordExpired(user))
        // {
        //     _logger.LogInformation("User with expired password sign in attempt");
        //     return AuthResult.PasswordChangeRequired;
        // }

        // var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, IsPersistent(), false);

        // if (result is CognitoSignInResult cognitoResult)
        // {
        //     if (cognitoResult.RequiresPasswordChange)
        //     {
        //         _logger.LogInformation("User password needs to be changed");
        //         return AuthResult.InitialPasswordSetRequired;
        //     }
        // }

        // if (!result.Succeeded)
        // {
        //     await _lockService.IncrementFailedSignInAttempt(user);
        //     _logger.LogInformation("Failed sign in attempt");
        //     return AuthResult.Failed(AuthErrorMessages.SignInErrorMessage);
        // }

        return AuthResult.Success;
    }
}
