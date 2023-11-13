using System;
using System.Text.Json;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Google.Cloud.Functions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Services.ComplianceCalculationService.Handler;

[FunctionsStartup(typeof(CalculationServiceStartup))]
public class Function : IHttpFunction
{
    private readonly ILogger<Function> _logger;
    private readonly AppDbContext _context;
    private readonly PostgresConfiguration _postgresConfiguration;
    public Function(AppDbContext context, ILogger<Function> logger,  IOptions<PostgresConfiguration> postgresConfiguration)
    {
        _context = context;
        _logger = logger;
        _postgresConfiguration = postgresConfiguration.Value;
    }

    public async Task HandleAsync(HttpContext context)
    {
        _logger.LogInformation("Started processing request.");
        var settingsJson = JsonSerializer.Serialize(_postgresConfiguration);
        _logger.LogInformation("Postgres settings: {settingsJson}", settingsJson);
        
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            if (canConnect)
            {
                _logger.LogInformation("Successfully connected to database.");
            }
            else
            {
                _logger.LogCritical("Failed to connect to database.");
            }
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while connecting to db.");
        }
        
        await context.Response.WriteAsync($"Hello!");
    }
}