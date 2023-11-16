using System;
using System.Text.Json;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zev.Core.Infrastructure.Logging;
using Zev.Core.Infrastructure.Persistence;
using Zev.Services.ComplianceCalculationService.Handler.Extensions;

namespace Zev.Services.ComplianceCalculationService.Handler;

public class Function : IHttpFunction
{
    private readonly ILogger _logger = SerilogHelper.GetLoggerFactory().CreateLogger<Function>();

    /// <summary>
    /// Logic for your function goes here.
    /// </summary>
    /// <param name="context">The HTTP context, containing the request and the response.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(HttpContext context)
    {
        _logger.LogInformation("Started processing request.");
        var settingsJson = JsonSerializer.Serialize(EnvironmentExtensions.GetPostgresSettings());
        _logger.LogInformation("Postgres settings: {@PostgresSettings}", settingsJson);


        try
        {
            await using var db = new AppDbContext(EnvironmentExtensions.GetPostgresSettings().ConnectionString);
            var created = await db.Database.EnsureCreatedAsync();
            _logger.LogInformation("Database created: {Created}", created);
            await context.Response.WriteAsync($"Db up and running: {created}");
        }
        catch (Exception e)
        {
            _logger.LogCritical("Database error {Error}",e);
            await context.Response.WriteAsync($"Db error: {e.Message}");
        }

        //await context.Response.WriteAsync($"Hello!");
    }
}
