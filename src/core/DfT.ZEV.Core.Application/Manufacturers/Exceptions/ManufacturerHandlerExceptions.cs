using DfT.ZEV.Common.Exceptions;

namespace DfT.ZEV.Core.Application.Manufacturers.Exceptions;

/// <summary>
/// Provides methods for creating exceptions related to manufacturer handlers.
/// </summary>
internal static class ManufacturerHandlerExceptions
{
    /// <summary>
    /// Creates a new handler exception indicating that a manufacturer already exists.
    /// </summary>
    /// <param name="name">The name of the manufacturer that already exists.</param>
    /// <returns>A new handler exception.</returns>
    public static HandlerException ManufacturerAlreadyExists(string name)
        => new($"Manufacturer with name {name} already exists");

    /// <summary>
    /// Creates a new entity not found exception indicating that a manufacturer was not found.
    /// </summary>
    /// <param name="id">The identifier of the manufacturer that was not found.</param>
    /// <returns>A new entity not found exception.</returns>
    public static EntityNotFoundException ManufacturerNotFound(Guid id)
        => new($"Manufacturer with id {id} not found");
}