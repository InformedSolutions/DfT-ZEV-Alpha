using System.IO;
using System.Threading.Tasks;

namespace Zev.Services.ComplianceCalculationService.Handler.Processing;

/// <summary>
/// Interface for processing strategies used by the compliance calculation service.
/// </summary>
public interface IProcessingService
{
    /// <summary>
    /// Processes the given stream and returns the result of the processing.
    /// </summary>
    /// <param name="stream">The stream to process.</param>
    /// <returns>The result of the processing.</returns>
    public Task<ProcessingResult> ProcessAsync(Stream stream, int chunkSize);
}