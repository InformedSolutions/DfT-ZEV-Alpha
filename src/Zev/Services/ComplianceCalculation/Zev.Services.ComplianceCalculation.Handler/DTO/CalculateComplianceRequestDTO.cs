namespace Zev.Services.ComplianceCalculation.Handler.DTO;

public class CalculateComplianceRequestDto
{
    public string FileName { get; set; }
    public int ChunkSize { get; set; }
}