using System;
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
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex,"Fatal error during migration.");
        }
        await context.Response.WriteAsync("Hello, Migrations.");
    }
}