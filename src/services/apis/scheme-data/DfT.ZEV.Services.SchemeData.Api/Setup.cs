using System.Text.Json.Serialization;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.MVC.Authentication.HealthChecks;
using DfT.ZEV.Core.Infrastructure;
using Microsoft.AspNetCore.Http.Json;
using DfT.ZEV.Core.Application;
using DfT.ZEV.Services.SchemeData.Api.Features.Processes;
using DfT.ZEV.Common.MVC.Authentication.HealthChecks.CustomHealthChecks;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using DfT.ZEV.Common.Logging;
using DfT.ZEV.Common.MVC.Authentication.Identity.Extensions;

namespace DfT.ZEV.Services.SchemeData.Api;

public static class Setup
{
    public static WebApplicationBuilder SetupServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks()
                 .AddCheck<PostgresHealthCheck>("postgres", HealthStatus.Unhealthy)
                 .AddCheck<RestServiceHealthCheck>("organization-api-service", HealthStatus.Unhealthy);

        builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.ConfigureServicesSettings(builder.Configuration);
        var postgresSettings = builder.Services.ConfigurePostgresSettings(builder.Configuration);
        builder.Services.ConfigureGoogleCloudSettings(builder.Configuration);
        builder.Services.AddIdentityPlatform(builder.Configuration);
        builder.Services.AddDbContext(postgresSettings);


        builder.UseCustomSerilog();

        builder.Services.AddApplication();

        builder.Services.AddRepositories();
        return builder;
    }

    public static WebApplication SetupWebApplication(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapProcessesEndpoints();
        app.MapVehicleEndpoints();

        app.UseHealthChecks();
        return app;
    }
}