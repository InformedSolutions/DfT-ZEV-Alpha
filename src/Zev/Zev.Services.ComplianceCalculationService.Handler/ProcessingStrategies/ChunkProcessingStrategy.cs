using System;
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
using Zev.Core.Infrastructure.Persistence;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Maps;


namespace Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

/// <summary>
/// Implements the <see cref="IProcessingStrategy"/> interface to process a stream of CSV data in chunks.
/// </summary>
public class ChunkProcessingStrategy : IProcessingStrategy
{
    private readonly ILogger _logger;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IList<RawVehicleDTO> _buffer;
    private int _counter;
    private int _bufferSize;
    private Stopwatch _stopwatch;
    public ChunkProcessingStrategy(AppDbContext context, ILogger logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _buffer = new List<RawVehicleDTO>(_bufferSize);
        _stopwatch = new Stopwatch();
    }


    /// <summary>
    /// Processes the given stream of CSV data in chunks, buffering records until a certain size is reached,
    /// then mapping and saving them to the database. Returns a <see cref="ProcessingResult"/> object indicating
    /// the success of the operation and the number of records processed.
    /// </summary>
    /// <param name="stream">The stream of CSV data to process.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result is a <see cref="ProcessingResult"/> object.</returns>
    public async Task<ProcessingResult> ProcessAsync(Stream stream, int chunkSize)
    {
        _bufferSize = chunkSize;
        _stopwatch.Start();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, GetCsvConfig());
        csv.Context.RegisterClassMap<RawVehicleCsvMap>();

        while (await csv.ReadAsync())
        {
            ProcessRecord(csv);
            await CheckAndProcessBuffer();
        }

        // Process any remaining records in the buffer
        if (_buffer.Count > 0)
        {
            await ProcessBuffer();
        }

        _stopwatch.Stop();

        return new ProcessingResult()
        {
            Success = true,
            Count = _counter,
            ProcessingTime = _stopwatch.ElapsedMilliseconds
        };
    }

    private void ProcessRecord(CsvReader csv)
    {
        try
        {
            var record = csv.GetRecord<RawVehicleDTO>();
            _buffer.Add(record);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error processing record {csv.CurrentIndex}");
            throw;
        }
    }

    private async Task CheckAndProcessBuffer()
    {
        if (_buffer.Count >= _bufferSize)
        {
            await ProcessBuffer();
        }
    }


    private async Task ProcessBuffer()
    {
        _logger.Information($"Processing chunk of {_buffer.Count} records");

        var mappedVehicles = _mapper.Map<IEnumerable<Vehicle>>(_buffer).ToList();
        await SaveVehicles(mappedVehicles);

        _counter += _buffer.Count;
        _buffer.Clear();
    }

    private async Task SaveVehicles(IEnumerable<Vehicle> vehicles)
    {
        try
        {
            //await _context.BulkInsertOrUpdateAsync(vehicles);

            //Base ef core insert, non optimized
            await _context.AddRangeAsync(vehicles);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error saving chunk of {_buffer.Count} records");
            throw;
        }
    }

    private static CsvConfiguration GetCsvConfig() =>
        new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
        };
}