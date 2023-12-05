using System.Text.Json.Serialization;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.Middlewares.ErrorHandling;
using DfT.ZEV.Common.MVC.Authentication.HealthChecks;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Application;
using DfT.ZEV.Core.Infrastructure;
using DfT.ZEV.Services.Organisations.Api.Features.Accounts;
using DfT.ZEV.Services.Organisations.Api.Features.Auth;
using DfT.ZEV.Services.Organisations.Api.Features.Manufacturers;
using DfT.ZEV.Services.Organisations.Api.Features.Permissions;
using Microsoft.AspNetCore.Http.Json;

namespace DfT.ZEV.Services.Organisations.Api;

public static class Setup
{
    public static WebApplicationBuilder SetupServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHealthCheckServices();

        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureGoogleCloudSettings(builder.Configuration);
        builder.Services.AddIdentityPlatform(builder.Configuration);
        builder.Services.AddTransient<RestExceptionHandlerMiddleware>();
        
        builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddApplication();
        var postgresSettings = builder.Services.ConfigurePostgresSettings(builder.Configuration);
        
        builder.Services.AddDbContext(postgresSettings);
        
        //to-do: add serilog from commons
        //builder.Services.AddSerilog(builder.Configuration);
        builder.Services.AddLogging();
        
        builder.Services.AddRepositories();
        return builder;
    }

    public static WebApplication SetupWebApplication(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<RestExceptionHandlerMiddleware>();
        app.MapAccountsEndpoints();
        app.MapManufacturerEndpoints();
        app.MapPermissionsEndpoints();
        app.UseHealthChecks();

        return app;
    }
}