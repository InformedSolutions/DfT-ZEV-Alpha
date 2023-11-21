using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Google.Cloud.Functions.Hosting;
using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Persistence;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Processing;

namespace Zev.Services.ComplianceCalculationService.Handler;

/// <summary>
/// Represents an HTTP function that handles requests to calculate compliance.
/// </summary>
[FunctionsStartup(typeof(ServiceStartup))]
public class Function : IHttpFunction
{
    public async Task HandleAsync(HttpContext context)
    {
        var res = new
        {
            Message = "This is fallback message",
        };
        var resJson = JsonSerializer.Serialize(res);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(resJson);
    }
    
}