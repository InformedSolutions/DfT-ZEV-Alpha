namespace Zev.Services.ComplianceCalculationService.Handler.ProcessingStrategies;

public class ProcessingResult
{
    public bool Success { get; set; }
    public int Count { get; set; }
    public long ProcessingTime { get; set; }
    public long ExecutionTime { get; set; }
    public string ExecutionId { get; set; }
    public int BufferCount { get; set; }
}