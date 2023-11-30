using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using DfT.ZEV.Core.Domain.Vehicles.Services;
using DfT.ZEV.Services.ComplianceCalculation.Handler.DTO;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.Processing;

/// <summary>
///     Implements the IProcessingStrategy interface to provide a strategy for processing data in fixed chunks.
/// </summary>
public class ChunkProcessingService : IProcessingService
{
    private readonly ConcurrentStack<RawVehicleDTO> _bufferStack = new();
    private readonly ILogger<ChunkProcessingService> _logger;
    private readonly IMapper _mapper;
    private readonly Stopwatch _stopwatch = new();
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleService _vehicleService;
    
    private int _bufferCounter;
    private int _recordCounter;
    public ChunkProcessingService(ILogger<ChunkProcessingService> logger, IUnitOfWork unitOfWork, IMapper mapper,
        IVehicleService vehicleService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _vehicleService = vehicleService;
    }

    /// <inheritdoc />
    public async Task<ProcessingResult> ProcessAsync(Stream stream, int chunkSize)
    {
        _logger.LogInformation("Processing started.");
        _stopwatch.Start();
        
        using var reader = new StreamReader(stream);
        var transactionId = await _unitOfWork.BeginTransactionAsync();

        _logger.LogInformation("Beginning transaction: {TransactionId}", transactionId);

        try
        {
            await ReadFromCsvAndProcessBuffer(reader, chunkSize);

            // Process the remaining records
            if (!_bufferStack.IsEmpty)
            {
                _logger.LogInformation("Processing remaining records in buffer.");
                await ProcessBuffer();
            }

            _logger.LogInformation("Committing transaction: {TransactionId}.", transactionId);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing file. Rolling back transaction: {TransactionId}.",
                transactionId);
            await _unitOfWork.RollbackTransactionAsync();
            return ProcessingResult.Fail(_recordCounter, _stopwatch.ElapsedMilliseconds, _bufferCounter);
        }

        _logger.LogInformation(
            "Processing completed. Processed {RecordCounter} records in {ElapsedMilliseconds} milliseconds.",
            _recordCounter, _stopwatch.ElapsedMilliseconds);
        return ProcessingResult.Successful(_recordCounter, _stopwatch.ElapsedMilliseconds, _bufferCounter);
    }

    private async Task ReadFromCsvAndProcessBuffer(StreamReader reader, int chunkSize)
    {
        using var csv = new CsvReader(reader, CsvHelpers.GetCsvConfig());
        CsvHelpers.ConfigureContext(csv.Context);


        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<RawVehicleDTO>();
            _bufferStack.Push(record);

            if (_bufferStack.Count >= chunkSize) await ProcessBuffer();
        }
    }

    private async Task ProcessBuffer()
    {
        var stopwatch = Stopwatch.StartNew();

        var stackCount = _bufferStack.Count;
        _logger.LogInformation("Processing buffer {BufferCounter} with {StackCount} records", _bufferCounter, stackCount);

        var mappedVehicles = _mapper.Map<IEnumerable<Vehicle>>(_bufferStack).ToList();
        _logger.LogInformation("Mapping took {ElapsedMilliseconds} milliseconds", stopwatch.ElapsedMilliseconds);

        stopwatch.Restart();
        //await Parallel.ForEachAsync(mappedVehicles, async (vehicle,ct) => await _vehicleService.ApplyRules(vehicle));
        await _vehicleService.ApplyRules(mappedVehicles);
        _logger.LogInformation("Applying rules took {ElapsedMilliseconds} milliseconds", stopwatch.ElapsedMilliseconds);

        stopwatch.Restart();
        await _unitOfWork.Vehicles.BulkInsertAsync(mappedVehicles);
        _logger.LogInformation("Inserting records took {ElapsedMilliseconds} milliseconds", stopwatch.ElapsedMilliseconds);


        _recordCounter += stackCount;
        _bufferCounter++;
        _bufferStack.Clear();
    }
}