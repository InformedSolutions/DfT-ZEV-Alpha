using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using FluentValidation;
using Serilog;
using Zev.Services.ComplianceCalculationService.Handler.DTO;
using Zev.Services.ComplianceCalculationService.Handler.Maps;

namespace Zev.Services.ComplianceCalculationService.Handler.Validation;

public class CsvValidatorService : ICsvValidatorService
{
    private readonly IValidator<RawVehicleDTO> _validator;
    private readonly ILogger _logger;

    private const int ErrorCap = 50;
    private int index = 0;
    public CsvValidatorService(ILogger logger)
    {
        _validator = new RawVehicleDTOValidator();
        _logger = logger;
    }

    public async Task<CsvValidationResponse> ValidateAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var errors = new List<ValidatorError>();
        using var csv = new CsvReader(reader, CsvHelper.GetCsvConfig());
        csv.Context.RegisterClassMap<RawVehicleCsvMap>();
        while (await csv.ReadAsync())
        {
            RawVehicleDTO record = null;
            index++;
            try
            {
                record = csv.GetRecord<RawVehicleDTO>();
            }
            catch (Exception ex)
            {
                errors.Add(new ValidatorError()
                {
                    Index = index,
                    Messages = new []{ex.Message},
                });
            }

            if (record is not null)
            {
                var result = await _validator.ValidateAsync(record);
                if (!result.IsValid)
                {
                    errors.Add(new ValidatorError()
                    {
                        Index = index, 
                        Messages = result.Errors.Select(x => x.ErrorMessage).ToArray()
                    });
                
                    _logger.Error("Validation failed for record {RecordNumber}. Errors: {Errors}", index, result.Errors);
                }
            }

            if (errors.Count >= ErrorCap)
            {
                break;
            }
        }
        return new CsvValidationResponse()
        {
            Errors = errors
        };
    }
}