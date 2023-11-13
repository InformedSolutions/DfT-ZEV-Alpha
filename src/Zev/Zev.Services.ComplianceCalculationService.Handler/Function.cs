using System;
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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Persistence;
using Zev.Services.ComplianceCalculationService.Handler.DTO;

namespace Zev.Services.ComplianceCalculationService.Handler;

[FunctionsStartup(typeof(CalculationServiceStartup))]
public class Function : IHttpFunction
{
    private readonly ILogger<Function> _logger;
    private readonly AppDbContext _context;
    private readonly PostgresConfiguration _postgresConfiguration;
    private readonly string _bucketName;
    public Function(AppDbContext context, ILogger<Function> logger,  IOptions<PostgresConfiguration> postgresConfiguration)
    {
        _context = context;
        _logger = logger;
        _postgresConfiguration = postgresConfiguration.Value;
        _bucketName = Environment.GetEnvironmentVariable("Manufacturer_Data_Bucket_Name") ?? throw new ArgumentNullException("ENV VAR Manufacturer_Data_Bucket_Name");
    }

    public async Task HandleAsync(HttpContext context)
    {
        var body = await GetRequestBody(context);
        
        _logger.LogInformation($"Requested processing file: {body.FileName} from bucket: {_bucketName}");
        
        var storage = await StorageClient.CreateAsync();
        var stream = new MemoryStream();
        await storage.DownloadObjectAsync(_bucketName, $"{body.FileName}", stream).ConfigureAwait(false);

        stream.Position = 0;
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, GetCsvConfig());
        csv.Context.RegisterClassMap<RawVehicleCsvMap>();

        while (await csv.ReadAsync())
        {
            try
            {
                var record = csv.GetRecord<RawVehicleDTO>();
                var json = JsonSerializer.Serialize(record);
               _logger.LogInformation($"Processing: {json}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing record {csv.CurrentIndex}");
            }
        }
        
        await context.Response.WriteAsync($"Hello!");
    }
    
    private static CsvConfiguration GetCsvConfig() => 
        new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
        };
    
    private static async Task<CalculateComplianceRequestDTO> GetRequestBody(HttpContext context)
    {
        using TextReader reader = new StreamReader(context.Request.Body);
        
        var json = await reader.ReadToEndAsync();
        var body = JsonSerializer.Deserialize<CalculateComplianceRequestDTO>(json);

        return body;
    }
}