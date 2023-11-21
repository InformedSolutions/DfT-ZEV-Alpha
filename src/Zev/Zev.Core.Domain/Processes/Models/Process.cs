using System.Text.Json;
using Zev.Core.Domain.Processes.Values;

namespace Zev.Core.Domain.Processes.Models;

public class Process
{
    public Guid Id { get; set; }
    
    public ProcessStatusEnum Status { get; set; }
    
    public string Name { get; set; }
    public JsonDocument? Metadata { get; set; }
    public JsonDocument? Result { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Process() { }
    
    public Process(Guid processId, string name)
    {
        Id = processId;
        Status = ProcessStatusEnum.Created;
        CreatedDate = DateTime.UtcNow;
        Name = name;
    }

    public Process Start()
    {
        StartDate = DateTime.UtcNow;
        Status = ProcessStatusEnum.InProgress;
        return this;
    }

    public Process Start(JsonDocument metadata)
    {
        Start();
        Metadata = metadata;
        return this;
    }

    public Process Finish()
    {
        EndDate = DateTime.UtcNow;
        Status = ProcessStatusEnum.Finished;
        return this;
    }
    
    public Process Finish(JsonDocument response)
    {
        Finish();
        Result = response;
        return this;
    }
    
    public Process Fail()
    {
        EndDate = DateTime.UtcNow;
        Status = ProcessStatusEnum.Failure;
        return this;
    }
    public Process Fail(JsonDocument response)
    {
        Fail();
        Result = response;
        return this;
    }
}