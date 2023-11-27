using System.Text.Json;
using DfT.ZEV.Core.Domain.Processes.Values;

namespace DfT.ZEV.Core.Domain.Processes.Models;

public sealed class Process
{
    public Guid Id { get; set; }

    public ProcessTypeEnum Type { get; set; }
    public ProcessStateEnum State { get; set; }

    public JsonDocument? Metadata { get; set; }
    public JsonDocument? Result { get; set; }

    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public DateTime? Started { get; set; }
    public DateTime? Finished { get; set; }

    public Process()
    {
    }

    public Process(Guid id, ProcessTypeEnum processType)
    {
        Id = id;
        Created = DateTime.UtcNow;
        State = ProcessStateEnum.Waiting;
        LastUpdated = DateTime.UtcNow;
        Type = processType;
    }

    public void Start(object? metadata = default)
    {
        State = ProcessStateEnum.Running;
        Started = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
        
        if(metadata is not null)
            Metadata = JsonSerializer.SerializeToDocument(metadata);
    }
    
    public void Finish(object? result = default)
    {
        State = ProcessStateEnum.Finished;
        Finished = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
        
        if(result is not null)
            Result = JsonSerializer.SerializeToDocument(result);
    }
    
    public void Fail(object? result = default)
    {
        State = ProcessStateEnum.Failed;
        Finished = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
        
        if(result is not null)
            Result = JsonSerializer.SerializeToDocument(result);
    }
}