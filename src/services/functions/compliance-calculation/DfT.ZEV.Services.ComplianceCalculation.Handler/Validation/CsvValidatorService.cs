using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using FluentValidation;
using DfT.ZEV.Services.ComplianceCalculation.Handler.DTO;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.Validation;

public class CsvValidatorService : ICsvValidatorService
{
    private const int ErrorCap = 50;
    private readonly ILogger<CsvValidatorService> _logger;
    private readonly IValidator<RawVehicleDTO> _validator;
    private int index;

    public CsvValidatorService(ILogger<CsvValidatorService> logger)
    {
        _validator = new RawVehicleDtoValidator();
        _logger = logger;
    }


    public async Task<CsvValidationResponse> ValidateAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var errors = new List<ValidatorError>();
        using var csv = new CsvReader(reader, CsvHelpers.GetCsvConfig());
        CsvHelpers.ConfigureContext(csv.Context);
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
                errors.Add(new ValidatorError
                {
                    Index = index,
                    Messages = new[] { ex.Message }
                });
                throw;
            }

            if (record is not null) await ValidateRecordAsync(record, errors);

            if (errors.Count >= ErrorCap) break;
        }

        return new CsvValidationResponse
        {
            Errors = errors
        };
    }

    private async Task ValidateRecordAsync(RawVehicleDTO record, List<ValidatorError> errors)
    {
        var result = await _validator.ValidateAsync(record);
        if (!result.IsValid)
        {
            errors.Add(new ValidatorError
            {
                Index = index,
                Messages = result.Errors.Select(x => x.ErrorMessage).ToArray()
            });

            _logger.LogError("Validation failed for record {RecordNumber}. Errors: {Errors}", index, result.Errors);
        }
    }
}