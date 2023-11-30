using DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;
using DfT.ZEV.Common.MVC.Authentication.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.MVC.Authentication.Services.Implementations;

public class ForgottenPasswordService : IForgottenPasswordService
{
    private readonly ILogger<ForgottenPasswordService> _logger;
  

    public ForgottenPasswordService(
        ILogger<ForgottenPasswordService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
    }

    public async Task<AuthResult> RequestForgottenPasswordChange(RequestPasswordChangeViewModel viewModel)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResult> VerifyForgottenPasswordToken(string token)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResult> ChangeForgottenPassword(ForgottenPasswordChangeViewModel viewModel)
    {
        throw new NotImplementedException();
    }
}
