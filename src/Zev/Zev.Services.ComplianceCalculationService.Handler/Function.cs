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
using Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

namespace Zev.Services.ComplianceCalculationService.Handler;

/// <summary>
/// Represents an HTTP function that handles requests to calculate compliance.
/// </summary>
[FunctionsStartup(typeof(ServiceStartup))]
public class Function : IHttpFunction
{
    private readonly ILogger _logger;
    private readonly IProcessingStrategy _processingStrategy;
    private readonly AppDbContext _context;
    private readonly BucketsConfiguration _bucketsConfiguration;


    public Function(AppDbContext context, ILogger logger, IProcessingStrategy processingStrategy, IOptions<BucketsConfiguration> bucketsConfiguration)
    {
        _context = context;
        _logger = logger;
        _processingStrategy = processingStrategy;
        _bucketsConfiguration = bucketsConfiguration.Value;
    }

    /// <summary>
    /// Handles an HTTP request to calculate compliance.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task HandleAsync(HttpContext context)
    {
        var executionId = Guid.NewGuid();

        using (LogContext.PushProperty("CorrelationId", executionId.ToString()))
        {
            await _context.Vehicles.ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            var body = await GetRequestBody(context);
       
            _logger.Information($"Requested processing file: {body.FileName} from bucket: {_bucketsConfiguration.ManufacturerImport}");
        
            var stopwatch = new Stopwatch(); stopwatch.Start();
            var storage = await StorageClient.CreateAsync(); 
            var stream = new MemoryStream(); 
            await storage.DownloadObjectAsync(_bucketsConfiguration.ManufacturerImport, $"{body.FileName}", stream).ConfigureAwait(false); 
            stream.Position = 0;
        
            var res = await _processingStrategy.ProcessAsync(stream, body.ChunkSize);
            
            stopwatch.Stop(); 
            res.ExecutionTime = stopwatch.ElapsedMilliseconds; 
            res.ExecutionId = executionId.ToString();
            
            _logger.Information("Finished processing file: {res}",res);

            var response = JsonSerializer.Serialize(res);
            context.Response.ContentType = "application/json"; 
            await context.Response.WriteAsync(response);

        }        
        
        
    }

    /// <summary>
    /// Gets the request body from the HTTP context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>The deserialized request body.</returns>
    private static async Task<CalculateComplianceRequestDto> GetRequestBody(HttpContext context)
    {
        using TextReader reader = new StreamReader(context.Request.Body);

        var json = await reader.ReadToEndAsync();
        var body = JsonSerializer.Deserialize<CalculateComplianceRequestDto>(json);

        return body;
    }
}