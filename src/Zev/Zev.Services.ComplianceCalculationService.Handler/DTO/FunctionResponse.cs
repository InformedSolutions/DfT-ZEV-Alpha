using System;

namespace Zev.Services.ComplianceCalculationService.Handler;

public class FunctionResponse
{
    public Guid ExecutionId { get; set; }
    public DateTime StartDate { get; set; }
}