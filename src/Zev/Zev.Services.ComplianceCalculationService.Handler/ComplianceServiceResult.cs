using System;
using Zev.Services.ComplianceCalculationService.Handler.Processing;

namespace Zev.Services.ComplianceCalculationService.Handler;

public sealed class ComplianceServiceResult
{
    public long ExecutionTime { get; set; }
    public bool Success { get; set; }
    public int Count { get; set; }
    public long ProcessingTime { get; set; }
    public int BufferCount { get; set; }

    public ComplianceServiceResult(ProcessingResult processingResult, long executionTime)
    {
        ExecutionTime = executionTime;
        Count = processingResult.Count;
        Success = processingResult.Success;
        ProcessingTime = processingResult.ProcessingTime;
        BufferCount = processingResult.BufferCount;
    }
}