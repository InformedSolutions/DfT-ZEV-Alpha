using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Zev.Core.Infrastructure.Persistence;
using Zev.Services.ComplianceCalculationService.Handler.DTO;

namespace Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

public class ChunkProcessingStrategy : IProcessingStrategy
{
    private readonly ILogger<ChunkProcessingStrategy> _logger;
    private readonly AppDbContext _context;

    private const int BUFFER_SIZE = 10;
    private int _counter = 0;
    private IList<RawVehicleDTO> _buffer = new List<RawVehicleDTO>(BUFFER_SIZE);
    
    public ChunkProcessingStrategy(AppDbContext context, ILogger<ChunkProcessingStrategy> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProcessingResult> ProcessAsync(Stream stream)
    {
        
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, GetCsvConfig());
        csv.Context.RegisterClassMap<RawVehicleCsvMap>();

        while (await csv.ReadAsync())
        {
            try
            {
                var record = csv.GetRecord<RawVehicleDTO>();
                _counter++;
                _buffer.Add(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing record {csv.CurrentIndex}");
            }

            if (_buffer.Count >= BUFFER_SIZE)
            {
                _logger.LogInformation($"Processing chunk of {_buffer.Count} records");
                //Processing logic
                _buffer.Clear();
            }
        }

        return new ProcessingResult()
        {
            Success = true,
            Count = _counter,
        };
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