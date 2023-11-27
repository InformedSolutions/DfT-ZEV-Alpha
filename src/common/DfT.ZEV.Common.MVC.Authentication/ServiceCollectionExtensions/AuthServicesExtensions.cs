using DfT.ZEV.Common.MVC.Authentication.Services;
using DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;
using idunno.Authentication.Basic;
using Informed.Common.Auth.Areas.Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DfT.ZEV.Common.MVC.Authentication.ServiceCollectionExtensions;

public static class AuthServicesExtensions
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<ISignOutService, SignOutService>();
        services.AddScoped<IForgottenPasswordService, ForgottenPasswordService>();
    }

    public static void AddBasicAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IBasicAuthUserValidationService, BasicAuthUserValidationService>();
        services
            .AddAuthentication()
            .AddBasic(options =>
            {
                options.AllowInsecureProtocol = true;
                options.Events = new BasicAuthenticationEvents
                {
                    OnValidateCredentials = context =>
                    {
                        var validationService =
                            context.HttpContext.RequestServices.GetService<IBasicAuthUserValidationService>();

                        if (validationService.AreCredentialsValid(context.Username, context.Password))
                        {
                            var claims = new[]
                            {
                                new Claim(
                                    ClaimTypes.NameIdentifier,
                                    context.Username,
                                    ClaimValueTypes.String,
                                    context.Options.ClaimsIssuer),
                                new Claim(
                                    ClaimTypes.Name,
                                    context.Username,
                                    ClaimValueTypes.String,
                                    context.Options.ClaimsIssuer),
                            };

                            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                            context.Success();
                        }

                        return Task.CompletedTask;
                    },
                };
            });
    }
}
