namespace Zev.Services.ComplianceCalculation.Handler.Processing;

public class ProcessingResult
{
    public bool Success { get; set; }
    public int Count { get; set; }
    public long ProcessingTime { get; set; }
    public int BufferCount { get; set; }

    public static ProcessingResult Fail(int count, long processingTime, int bufferCount) => new ProcessingResult()
    {
        Success = false,
        Count = count,
        ProcessingTime = processingTime,
        BufferCount = bufferCount
    };
    
    public static ProcessingResult Successful(int count, long processingTime, int bufferCount) => new ProcessingResult()
    {
        Success = true,
        Count = count,
        ProcessingTime = processingTime,
        BufferCount = bufferCount
    };
}