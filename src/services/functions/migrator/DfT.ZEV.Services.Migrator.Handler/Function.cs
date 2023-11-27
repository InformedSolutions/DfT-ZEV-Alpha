using System;
using System.Threading.Tasks;
using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Zev.Services.Migrator.Handler;

[FunctionsStartup(typeof(MigratorStartup))]
public class Function : IHttpFunction
{
    private readonly AppDbContext _context;
    private readonly ILogger<Function> _logger;

    public Function(AppDbContext context, ILogger<Function> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext context)
    {
        try
        {
            var alreadyAppliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

            await _context.Database.MigrateAsync();

            await context.Response.WriteAsJsonAsync(new MigratorResult
            {
                MigrationsAlreadyApplied = alreadyAppliedMigrations,
                MigrationsAppliedInCurrentRun = pendingMigrations
            });
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Fatal error during migration.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Migration failed.");
        }
    }
}