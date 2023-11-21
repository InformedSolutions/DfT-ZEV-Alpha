using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Processing;
using Process = Zev.Core.Domain.Processes.Models.Process;

namespace Zev.Services.ComplianceCalculationService.Handler;

public record ComplianceResponse(Guid processId, string message);

public class ComplianceService 
{
    private readonly ILogger _logger;
    private readonly IProcessingService _processingService;
    private readonly AppDbContext _context;
    private readonly BucketsConfiguration _bucketsConfiguration;
    private readonly IUnitOfWork _unitOfWork;

    public ComplianceService(AppDbContext context, ILogger logger, IProcessingService processingService, IOptions<BucketsConfiguration> bucketsConfiguration, IUnitOfWork unitOfWork)
    {
        _context = context;
        _logger = logger;
        _processingService = processingService;
        _unitOfWork = unitOfWork;
        _bucketsConfiguration = bucketsConfiguration.Value;
    }

    public async Task<ComplianceResponse> HandleAsync(CalculateComplianceRequestDto request)
    {
        var processId = Guid.NewGuid();
        var process = new Process(processId, "Compliance Import");
        await _unitOfWork.Processes.AddProcessAsync(process);
        await _unitOfWork.SaveChangesAsync();
        Run(request, process);

        return new ComplianceResponse(processId, "Process started!");
    }
    
    public async Task<ComplianceServiceResponse> Run(CalculateComplianceRequestDto body, Process process)
    {
        var executionId = process.Id;
        
        using (LogContext.PushProperty("CorrelationId", executionId.ToString()))
        {
            //await ClearVehiclesFromDatabase();

            _logger.Information($"Requested processing file: {body.FileName} from bucket: {_bucketsConfiguration.ManufacturerImport}");
            var jsonDocument = JsonSerializer.SerializeToDocument(body);
            process.Start(jsonDocument);

            await _unitOfWork.Processes.UpdateProcessAsync(process);
            await _unitOfWork.SaveChangesAsync();
            
            var stopwatch = StartStopwatch();

            var stream = await DownloadFileFromStorage(body);
            var processingResult = await _processingService.ProcessAsync(stream, body.ChunkSize);

            stopwatch.Stop();

            var res = GetResponse(processingResult, stopwatch.ElapsedMilliseconds, executionId);

            jsonDocument = JsonSerializer.SerializeToDocument(res);
            process.Finish(jsonDocument);
            await _unitOfWork.Processes.UpdateProcessAsync(process);
            await _unitOfWork.SaveChangesAsync();
            
            return res;
        }
    }

    private async Task ClearVehiclesFromDatabase()
    {
        //await _context.Vehicles.ExecuteDeleteAsync();
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Vehicles CASCADE");

        await _context.SaveChangesAsync();
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

    private ComplianceServiceResponse GetResponse(ProcessingResult processingResult, long elapsedMilliseconds, Guid executionId)
    {
        var response = new ComplianceServiceResponse(processingResult, elapsedMilliseconds, executionId);
        var resJson = JsonSerializer.Serialize(response);
        _logger.Information("Finished processing file: {resJson}", resJson);

        return response;
    }
}