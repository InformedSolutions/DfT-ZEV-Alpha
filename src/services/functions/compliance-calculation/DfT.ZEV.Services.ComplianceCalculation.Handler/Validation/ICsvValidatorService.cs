using System.IO;
using System.Threading.Tasks;

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.Validation;

public interface ICsvValidatorService
{
    Task<CsvValidationResponse> ValidateAsync(Stream stream);
}