using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public static class IdentityExtensions
{
    public static void AddIdentityPlatform(this IServiceCollection services, IConfiguration configuration)
    {
        var googleConfig = configuration.GetGoogleCloudSettings();
        services.AddAuthorization();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie("Cookie")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = googleConfig.Token.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = googleConfig.Token.Audience,
                    ValidateAudience = true,
                };
            });

        services.AddTransient<TokenMiddleware>();
        services.AddHttpClient<IGoogleIdentityApiClient, GoogleIdentityApiClient>();
        services.AddTransient<IIdentityPlatform, IdentityPlatform>();
    }

    public static IApplicationBuilder UseIdentity(this IApplicationBuilder app)
    {
        app.UseSession();

        //add token to request header.
        app.Use(async (context, next) =>
        {
            var token = context.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }
            await next();
        });
        app.UseMiddleware<TokenMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        
        return app;
    }
    
}