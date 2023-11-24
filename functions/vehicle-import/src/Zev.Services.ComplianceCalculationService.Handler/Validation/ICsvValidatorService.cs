using System.IO;
using System.Threading.Tasks;

namespace Zev.Services.ComplianceCalculationService.Handler.Validation;

public interface ICsvValidatorService
{
     Task<CsvValidationResponse> ValidateAsync(Stream stream);
}