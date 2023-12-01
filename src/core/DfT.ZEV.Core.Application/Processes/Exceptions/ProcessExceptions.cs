using DfT.ZEV.Common.Exceptions;

namespace DfT.ZEV.Core.Application.Processes.Exceptions;

public static class ProcessExceptions 
{
    public static HandlerException NotFound(Guid  id) => new ($"Process with id: {id} not found");
}