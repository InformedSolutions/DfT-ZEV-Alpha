using System.Text.Json.Serialization;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Infrastructure;
using Microsoft.AspNetCore.Http.Json;

using DfT.ZEV.Services.SchemeData.Api.Features.Processes;

namespace DfT.ZEV.Services.SchemeData.Api;

public static class Setup
{
    public static WebApplicationBuilder SetupServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();

        builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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
        
        app.MapProcessesEndpoints();
        
        app.MapHealthChecks("/healthz");

        return app;
    }
}