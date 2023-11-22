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
    private readonly ILogger _logger;
    private readonly IProcessingService _processingService;
    private readonly AppDbContext _context;
    private readonly BucketsConfiguration _bucketsConfiguration;


    public Function(AppDbContext context, ILogger logger, IProcessingService processingService, IOptions<BucketsConfiguration> bucketsConfiguration)
    {
        _context = context;
        _logger = logger;
        _processingService = processingService;
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
            await ClearVehiclesFromDatabase();

            var body = await GetRequestBody(context);
            _logger.Information($"Requested processing file: {body.FileName} from bucket: {_bucketsConfiguration.ManufacturerImport}");

            var stopwatch = StartStopwatch();

            var stream = await DownloadFileFromStorage(body);
            var processingResult = await _processingService.ProcessAsync(stream, body.ChunkSize);

            stopwatch.Stop();

            await WriteResponse(context, processingResult, stopwatch.ElapsedMilliseconds, executionId);
        }
    }

    private async Task ClearVehiclesFromDatabase()
    {
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE public.\"Vehicles\" CASCADE;");
        await _context.SaveChangesAsync();
        await _context.Database.ExecuteSqlRawAsync("VACUUM ANALYZE;");
    }

    private Stopwatch StartStopwatch()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        return stopwatch;
    }

    private async Task<MemoryStream> DownloadFileFromStorage(CalculateComplianceRequestDto body)
    {
        var storage = await StorageClient.CreateAsync();
        var stream = new MemoryStream();
        await storage.DownloadObjectAsync(_bucketsConfiguration.ManufacturerImport, $"{body.FileName}", stream).ConfigureAwait(false);
        stream.Position = 0;
        return stream;
    }

    private async Task WriteResponse(HttpContext context, ProcessingResult processingResult, long elapsedMilliseconds, Guid executionId)
    {
        var response = new ComplianceServiceResponse(processingResult, elapsedMilliseconds, executionId);
        var resJson = JsonSerializer.Serialize(response);
        _logger.Information("Finished processing file: {resJson}", resJson);

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(resJson);
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