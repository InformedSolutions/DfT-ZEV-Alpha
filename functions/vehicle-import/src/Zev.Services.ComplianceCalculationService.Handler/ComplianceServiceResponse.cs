using System;
using Zev.Services.ComplianceCalculationService.Handler.Processing;

namespace Zev.Services.ComplianceCalculationService.Handler;

public sealed class ComplianceServiceResponse
{
    public long ExecutionTime { get; set; }
    public Guid ExecutionId { get; set; }
    public bool Success { get; set; }
    public int Count { get; set; }
    public long ProcessingTime { get; set; }
    public int BufferCount { get; set; }

    public ComplianceServiceResponse(ProcessingResult processingResult, long executionTime, Guid executionId)
    {
        ExecutionTime = executionTime;
        ExecutionId = executionId;
        Count = processingResult.Count;
        Success = processingResult.Success;
        ProcessingTime = processingResult.ProcessingTime;
        BufferCount = processingResult.BufferCount;
    }
}