using System.Text.Json;
using Zev.Core.Domain.Processes.Values;

namespace Zev.Core.Domain.Processes.Models;

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

    public Process() {}
    public Process(Guid id, ProcessTypeEnum processType)
    {
        Id = id;
        Created = DateTime.UtcNow;
        State = ProcessStateEnum.Waiting;
        LastUpdated = DateTime.UtcNow;
        Type = processType;
    }
    
    public void Start()
    {
        State = ProcessStateEnum.Running;
        Started = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
    }

    public void Start(JsonDocument metadata)
    {
        Start();
        Metadata = metadata;
    }

    public void Finish()
    {
        State = ProcessStateEnum.Finished;
        Finished = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
    }
    
    public void Finish(JsonDocument result)
    {
        Finish();
        Result = result;
    }
    
    public void Fail()
    {
        State = ProcessStateEnum.Failed;
        Finished = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
    }
    
    public void Fail(JsonDocument result)
    {
        Fail();
        Result = result;
    }
}