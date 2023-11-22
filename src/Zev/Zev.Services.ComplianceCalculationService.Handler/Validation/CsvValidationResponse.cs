using System.Collections.Generic;

namespace Zev.Services.ComplianceCalculationService.Handler.Validation;

public class ValidatorError
{
    public int Index { get; set; }
    public string[] Messages { get; set; }
}

public class CsvValidationResponse
{
    public IEnumerable<ValidatorError> Errors { get; set; }
}