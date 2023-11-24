using System.Collections.Generic;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Zev.Services.ComplianceCalculation.Handler.Validation;

public class ValidatorError
{
    public int Index { get; set; }
    public string[] Messages { get; set; }
}

public class CsvValidationResponse
{
    public IEnumerable<ValidatorError> Errors { get; set; }
}