using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using FluentValidation;
using Serilog;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Maps;
using Zev.Services.ComplianceCalculationService.Handler.Validators;

namespace Zev.Services.ComplianceCalculationService.Handler.Processing;

public class CsvValidatorService
{
    private readonly IValidator<RawVehicleDTO> _validator;
    private readonly ILogger _logger;

    public CsvValidatorService(ILogger logger)
    {
        _validator = new RawVehicleDTOValidator();
        _logger = logger;
    }

    public async Task ValidateCsv(Stream stream)
    {
        using var reader = new StreamReader(stream);

        using var csv = new CsvReader(reader, CsvHelper.GetCsvConfig());
        csv.Context.RegisterClassMap<RawVehicleCsvMap>();
        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<RawVehicleDTO>();
            var result = await _validator.ValidateAsync(record);
            if (!result.IsValid)
            {
                _logger.Error("Validation failed for record {RecordNumber}. Errors: {Errors}", csv.CurrentIndex, result.Errors);
            }
        }
    }
}