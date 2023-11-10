using System;
using System.Text.Json;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Google.Cloud.Functions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zev.Core.Infrastructure.Logging;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Services.ComplianceCalculationService.Handler;

[FunctionsStartup(typeof(CalculationServiceStartup))]
public class Function : IHttpFunction
{
    private readonly ILogger _logger = SerilogHelper.GetLoggerFactory().CreateLogger<Function>();
    private readonly AppDbContext _context;

    public Function(AppDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(HttpContext context)
    {
        _logger.LogInformation("Started processing request.");
        
        var canConnect = await _context.Database.CanConnectAsync();

        if (canConnect)
        {
            _logger.LogInformation("Successfully connected to database.");
        }
        else
        {
            _logger.LogCritical("Failed to connect to database.");
        }
        
        await context.Response.WriteAsync($"Hello!");
    }
}