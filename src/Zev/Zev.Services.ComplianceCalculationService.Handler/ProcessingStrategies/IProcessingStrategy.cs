using System.IO;
using System.Threading.Tasks;

namespace Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

public interface IProcessingStrategy
{
    public Task<ProcessingResult> ProcessAsync(Stream stream);
}