using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Serilog;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Maps;

namespace Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

/// <summary>
/// Implements the IProcessingStrategy interface to provide a strategy for processing data in fixed chunks.
/// </summary>
public class FixedChunkProcessingStrategy : IProcessingStrategy
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly ConcurrentStack<RawVehicleDTO> _bufferStack = new ConcurrentStack<RawVehicleDTO>();

    private int _recordCounter = 0;
    private int _bufferCounter = 0;

    public FixedChunkProcessingStrategy(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Processes the given stream in fixed-size chunks.
    /// </summary>
    /// <param name="stream">The stream to process.</param>
    /// <param name="chunkSize">The size of the chunks to process.</param>
    /// <returns>A ProcessingResult indicating the result of the processing.</returns>
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

            _logger.Information("Committing transaction.");
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing file. Rolling back transaction.");
            await transaction.RollbackAsync();
            return ProcessingResult.Fail(_recordCounter, _stopwatch.ElapsedMilliseconds, _bufferCounter);
        }

        _logger.Information("Processing completed. Processed {RecordCounter} records in {ElapsedMilliseconds} milliseconds.", _recordCounter, _stopwatch.ElapsedMilliseconds);
        return ProcessingResult.Successful(_recordCounter, _stopwatch.ElapsedMilliseconds, _bufferCounter);
    }

    private async Task ReadFromCsvAndProcessBuffer(StreamReader reader, int chunkSize)
    {
        using var csv = new CsvReader(reader, GetCsvConfig());
        csv.Context.RegisterClassMap<RawVehicleCsvMap>();

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
        var stackCount = _bufferStack.Count;
        _logger.Information("Processing buffer {BufferCounter} with {StackCount} records", _bufferCounter, stackCount);
        var mappedVehicles = _mapper.Map<IEnumerable<Vehicle>>(_bufferStack);

        await _unitOfWork.Vehicles.BulkInsertAsync(mappedVehicles);

        _recordCounter += stackCount;
        _bufferCounter++;
        _bufferStack.Clear();
    }

    private static CsvConfiguration GetCsvConfig() =>
        new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
        };
}