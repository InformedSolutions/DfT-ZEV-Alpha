using DfT.ZEV.Core.Application.Common;

namespace DfT.ZEV.Core.Application.Accounts.Exceptions;

public static class ManufacturerHandlerExceptions
{
    public static HandlerException ManufacturerAlreadyExists(string name) 
        => new($"Manufacturer with name {name} already exists");
    
}