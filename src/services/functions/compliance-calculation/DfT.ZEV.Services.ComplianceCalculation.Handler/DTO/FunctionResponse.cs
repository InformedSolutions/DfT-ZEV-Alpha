using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.DTO;

public class FunctionResponse
{
    public Guid ExecutionId { get; set; }
    public DateTime StartDate { get; set; }
}