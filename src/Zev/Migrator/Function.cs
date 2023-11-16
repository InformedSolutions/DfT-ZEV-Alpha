using System;
using System.Collections.Generic;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Google.Cloud.Functions.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Zev.Core.Infrastructure.Persistence;

namespace Migrator;

[FunctionsStartup(typeof(MigratorStartup))]
public class Function : IHttpFunction
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    public Function(AppDbContext context, ILogger logger)
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
            _logger.Fatal(ex, "Fatal error during migration.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Migration failed.");
        }
    }
}
