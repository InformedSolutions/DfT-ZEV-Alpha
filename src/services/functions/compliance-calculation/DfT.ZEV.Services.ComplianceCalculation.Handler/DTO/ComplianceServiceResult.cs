using DfT.ZEV.Services.ComplianceCalculation.Handler.Processing;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.DTO;

public sealed class ComplianceServiceResult
{
    public ComplianceServiceResult(ProcessingResult processingResult, long executionTime)
    {
        ExecutionTime = executionTime;
        Count = processingResult.Count;
        Success = processingResult.Success;
        ProcessingTime = processingResult.ProcessingTime;
        BufferCount = processingResult.BufferCount;
    }

    public long ExecutionTime { get; set; }
    public bool Success { get; set; }
    public int Count { get; set; }
    public long ProcessingTime { get; set; }
    public int BufferCount { get; set; }
}