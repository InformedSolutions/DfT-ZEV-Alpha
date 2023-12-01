using System.Text.Json;
using DfT.ZEV.Core.Domain.Processes.Values;

namespace DfT.ZEV.Core.Domain.Processes.Models;

public sealed class Process
{
    public Guid Id { get; set; }

    public ProcessType Type { get; set; }
    public ProcessState State { get; set; }

    public JsonDocument? Metadata { get; set; }
    public JsonDocument? Result { get; set; }

    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public DateTime? Started { get; set; }
    public DateTime? Finished { get; set; }

    public Process()
    {
    }

    public Process(Guid id, ProcessType processType)
    {
        Id = id;
        Created = DateTime.UtcNow;
        State = ProcessState.Waiting;
        LastUpdated = DateTime.UtcNow;
        Type = processType;
    }

    public void Start(object? metadata = default)
    {
        State = ProcessState.Running;
        Started = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
        
        if(metadata is not null)
            Metadata = JsonSerializer.SerializeToDocument(metadata);
    }
    
    public void Finish(object? result = default)
    {
        State = ProcessState.Finished;
        Finished = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
        
        if(result is not null)
            Result = JsonSerializer.SerializeToDocument(result);
    }
    
    public void Fail(object? result = default)
    {
        State = ProcessState.Failed;
        Finished = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
        
        if(result is not null)
            Result = JsonSerializer.SerializeToDocument(result);
    }
}