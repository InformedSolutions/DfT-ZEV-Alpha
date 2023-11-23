using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Zev.Core.Domain.Processes.Values;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ComplianceCalculation.Handler.DTO;
using Zev.Services.ComplianceCalculation.Handler.Processing;
using Zev.Services.ComplianceCalculation.Handler.Validation;
using Process = Zev.Core.Domain.Processes.Models.Process;

namespace Zev.Services.ComplianceCalculation.Handler;

/// <summary>
///     Represents an HTTP function that handles requests to calculate compliance.
/// </summary>
[FunctionsStartup(typeof(ServiceStartup))]
public class Function : IHttpFunction
{
    private readonly BucketsConfiguration _bucketsConfiguration;
    private readonly AppDbContext _context;
    private readonly CsvValidatorService _csvValidatorService;
    private readonly ILogger _logger;
    private readonly IProcessingService _processingService;
    private readonly IUnitOfWork _unitOfWork;

    public Function(ILogger logger, IProcessingService processingService,
        IOptions<BucketsConfiguration> bucketsConfiguration, CsvValidatorService csvValidatorService,
        IUnitOfWork unitOfWork, AppDbContext context)
    {
        _logger = logger;
        _processingService = processingService;
        _csvValidatorService = csvValidatorService;
        _unitOfWork = unitOfWork;
        _context = context;
        _bucketsConfiguration = bucketsConfiguration.Value;
    }

    /// <summary>
    ///     Handles an HTTP request to calculate compliance.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task HandleAsync(HttpContext context)
    {
        var executionId = Guid.NewGuid();
        var body = await GetRequestBody(context);
        var process = new Process(executionId, ProcessTypeEnum.ComplianceDataImport);
        process.Start(JsonSerializer.SerializeToDocument(body));
        using (LogContext.PushProperty("CorrelationId", executionId.ToString()))
        {
            await _unitOfWork.Processes.AddAsync(process);
            await _unitOfWork.SaveChangesAsync();

            Run(body, process).ConfigureAwait(false);
        }

        var res = new FunctionResponse
        {
            ExecutionId = executionId,
            StartDate = process.Created
        };

        var resJson = JsonSerializer.Serialize(res);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(resJson);
    }

    private async Task Run(CalculateComplianceRequestDto body, Process process)
    {
        _logger.Information(
            $"Requested processing file: {body.FileName} from bucket: {_bucketsConfiguration.ManufacturerImport}");

        var stopwatch = StartStopwatch();

        _logger.Information("Starting truncation of vehicle data");
        await ClearVehiclesFromDatabase();
        _logger.Information($"Vehicle data successfully truncated after {stopwatch.ElapsedMilliseconds}ms");


        var stream = await DownloadFileFromStorage(body);

        var validationResult = await _csvValidatorService.ValidateAsync(stream);
        if (validationResult.Errors.Any())
        {
            await HandleValidationErrors(validationResult, process, stopwatch);
            return;
        }

        //There might be memory spikes here. Need to test this.
        stream = new MemoryStream(stream.ToArray());
        var processingResult = await _processingService.ProcessAsync(stream, body.ChunkSize);
        stopwatch.Stop();
        await HandleProcessingResult(processingResult, process, stopwatch);
    }

    private async Task ClearVehiclesFromDatabase()
    {
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE public.\"Vehicles\" CASCADE;");
        await _context.SaveChangesAsync();
        await _context.Database.ExecuteSqlRawAsync("VACUUM ANALYZE;");
    }

    private async Task HandleValidationErrors(CsvValidationResponse validationResult, Process process,
        Stopwatch stopwatch)
    {
        stopwatch.Stop();
        process.Fail(JsonSerializer.SerializeToDocument(validationResult));
        _unitOfWork.Processes.Update(process);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task HandleProcessingResult(ProcessingResult processingResult, Process process, Stopwatch stopwatch)
    {
        var response = new ComplianceServiceResult(processingResult, stopwatch.ElapsedMilliseconds);
        if (response.Success)
        {
            process.Finish(JsonSerializer.SerializeToDocument(response));
            _unitOfWork.Processes.Update(process);
            await _unitOfWork.SaveChangesAsync();
            var resJson = JsonSerializer.Serialize(response);
            _logger.Information("Finished processing file: {resJson}", resJson);
        }
        else
        {
            process.Fail(JsonSerializer.SerializeToDocument(response));
            _unitOfWork.Processes.Update(process);
            await _unitOfWork.SaveChangesAsync();
            var resJson = JsonSerializer.Serialize(response);
            _logger.Information("Failed processing file: {resJson}", resJson);
        }
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
        await storage.DownloadObjectAsync(_bucketsConfiguration.ManufacturerImport, $"{body.FileName}", stream)
            .ConfigureAwait(false);
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    ///     Gets the request body from the HTTP context.
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