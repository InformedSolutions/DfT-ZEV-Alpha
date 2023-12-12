using DfT.ZEV.Common.Exceptions;

namespace DfT.ZEV.Core.Application.Processes.Exceptions;

/// <summary>
/// Creates a new entity not found exception indicating that a manufacturer was not found.
/// </summary>
/// <param name="id">The identifier of the manufacturer that was not found.</param>
/// <returns>A new entity not found exception.</returns>
internal static class ProcessExceptions
{
    /// <summary>
    /// Creates a new handler exception indicating that a process was not found.
    /// </summary>
    /// <param name="id">The identifier of the process that was not found.</param>
    /// <returns>A new handler exception.</returns>
    public static HandlerException NotFound(Guid id) => new($"Process with id: {id} not found");
}