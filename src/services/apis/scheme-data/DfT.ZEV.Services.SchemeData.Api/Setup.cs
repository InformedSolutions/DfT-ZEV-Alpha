using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using DfT.ZEV.Core.Infrastructure.Configuration;
using DfT.ZEV.Core.Infrastructure.Logging;
using DfT.ZEV.Core.Infrastructure.Persistence;
using DfT.ZEV.Core.Infrastructure.Repositories;
using DfT.ZEV.Services.SchemeData.Api.Features.Processes;

namespace DfT.ZEV.Services.SchemeData.Api;

public static class Setup
{
    public static WebApplicationBuilder SetupServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        var postgresSettings = builder.Services.ConfigurePostgresSettings(builder.Configuration);

        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(postgresSettings.ConnectionString,
                conf => { conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), new List<string> { "4060" }); });
        });
        builder.Services.AddSerilog(builder.Configuration);
        builder.Services.AddRepositories();
        return builder;
    }

    public static WebApplication SetupWebApplication(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapProcessesEndpoints();
        return app;
    }
}