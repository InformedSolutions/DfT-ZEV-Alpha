using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Serilog;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Domain.Vehicles.Models;
using Zev.Core.Domain.Vehicles.Services;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Maps;
using Zev.Services.ComplianceCalculationService.Handler.Validation;

namespace Zev.Services.ComplianceCalculationService.Handler.Processing;

/// <summary>
/// Implements the IProcessingStrategy interface to provide a strategy for processing data in fixed chunks.
/// </summary>
public class ChunkProcessingService : IProcessingService
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IVehicleService _vehicleService;

    private readonly RawVehicleDTOValidator _validator = new();
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly ConcurrentStack<RawVehicleDTO> _bufferStack = new ConcurrentStack<RawVehicleDTO>();

    private int _recordCounter = 0;
    private int _bufferCounter = 0;

    public ChunkProcessingService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, IVehicleService vehicleService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _vehicleService = vehicleService;
    }

    /// <inheritdoc/>
    public async Task<ProcessingResult> ProcessAsync(Stream stream, int chunkSize)
    {
        _logger.Information("Processing started.");
        _stopwatch.Start();

        
        using var reader = new StreamReader(stream);
        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        _logger.Information("Beginning transaction: {TransactionId}", transaction.TransactionId);

        try
        {
            await ReadFromCsvAndProcessBuffer(reader, chunkSize);

            // Process the remaining records
            if (!_bufferStack.IsEmpty)
            {
                _logger.Information("Processing remaining records in buffer.");
                await ProcessBuffer();
            }

            _logger.Information("Committing transaction: {TransactionId}.", transaction.TransactionId);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing file. Rolling back transaction: {TransactionId}.", transaction.TransactionId);
            await transaction.RollbackAsync();
            return ProcessingResult.Fail(_recordCounter, _stopwatch.ElapsedMilliseconds, _bufferCounter);
        }

        _logger.Information("Processing completed. Processed {RecordCounter} records in {ElapsedMilliseconds} milliseconds.", _recordCounter, _stopwatch.ElapsedMilliseconds);
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

            if (_bufferStack.Count >= chunkSize)
            {
                await ProcessBuffer();
            }
        }
    }
    private async Task ProcessBuffer()
    {
        var stopwatch = Stopwatch.StartNew();

        var stackCount = _bufferStack.Count;
        _logger.Information("Processing buffer {BufferCounter} with {StackCount} records", _bufferCounter, stackCount);

        var mappedVehicles = _mapper.Map<IEnumerable<Vehicle>>(_bufferStack).ToList();
        _logger.Information("Mapping took {ElapsedMilliseconds} milliseconds", stopwatch.ElapsedMilliseconds);

        stopwatch.Restart();
        Parallel.ForEach(mappedVehicles, vehicle => _vehicleService.ApplyRules(vehicle));
        _logger.Information("Applying rules took {ElapsedMilliseconds} milliseconds", stopwatch.ElapsedMilliseconds);

        stopwatch.Restart();
        await _unitOfWork.Vehicles.BulkInsertAsync(mappedVehicles);
        _logger.Information("Inserting records took {ElapsedMilliseconds} milliseconds", stopwatch.ElapsedMilliseconds);


        _recordCounter += stackCount;
        _bufferCounter++;
        _bufferStack.Clear();
    }
}