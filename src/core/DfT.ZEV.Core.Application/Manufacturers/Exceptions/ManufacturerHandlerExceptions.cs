using DfT.ZEV.Common.Exceptions;

namespace DfT.ZEV.Core.Application.Manufacturers.Exceptions;

public static class ManufacturerHandlerExceptions
{
    public static HandlerException ManufacturerAlreadyExists(string name) 
        => new($"Manufacturer with name {name} already exists");
    
}