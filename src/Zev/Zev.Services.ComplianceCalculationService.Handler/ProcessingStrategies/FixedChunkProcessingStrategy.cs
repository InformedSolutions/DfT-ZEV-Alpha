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
using Serilog;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Maps;

namespace Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

public class FixedChunkProcessingStrategy : IProcessingStrategy
{
    //DI
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    //Fields
    private readonly Stopwatch _stopwatch;
    private readonly ConcurrentStack<RawVehicleDTO> _bufferStack;
    private int _recordCounter;
    private int _bufferCounter;
    public FixedChunkProcessingStrategy(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _stopwatch = new Stopwatch();
        _bufferStack = new ConcurrentStack<RawVehicleDTO>();
        _recordCounter = 0;
        _bufferCounter = 0;
    }
    public async Task<ProcessingResult> ProcessAsync(Stream stream, int chunkSize)
    {
        _stopwatch.Start();

        using var reader = new StreamReader(stream);
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        _logger.Information("Beginning transaction: {TransactionId}", transaction.TransactionId);
        try
        {
            using (var csv = new CsvReader(reader, GetCsvConfig()))
            {
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

            //Process the remaining records
            if (!_bufferStack.IsEmpty)
                await ProcessBuffer();
            
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.Error(ex, "Error processing file");
            return ProcessingResult.Fail(_recordCounter, _stopwatch.ElapsedMilliseconds, _bufferCounter);
        }

        _stopwatch.Stop();
        
        return ProcessingResult.Successful(_recordCounter, _stopwatch.ElapsedMilliseconds, _bufferCounter);
    }

    private async Task ProcessBuffer()
    {
        var stackCount = _bufferStack.Count;
        //_logger.Information("Processing buffer {BufferCounter} with {StackCount} records", _bufferCounter, stackCount);
        var mappedVehicles = _mapper.Map<IEnumerable<Vehicle>>(_bufferStack);
        
        //await _context.AddRangeAsync(mappedVehicles); 
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