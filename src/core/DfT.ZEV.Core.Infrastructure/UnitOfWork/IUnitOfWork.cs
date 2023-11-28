using DfT.ZEV.Core.Domain.Accounts.Services;
using Microsoft.EntityFrameworkCore.Storage;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;

namespace DfT.ZEV.Core.Infrastructure.UnitOfWork;

/// <summary>
///     Represents a unit of work for managing database operations.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Gets the repository for managing vehicle entities.
    /// </summary>
    IVehicleRepository Vehicles { get; }

    /// <summary>
    ///     Gets the repository for managing process entities.
    /// </summary>
    IProcessRepository Processes { get; }

    IUserRepository Users { get; }
    /// <summary>
    ///     Saves all changes made in the unit of work to the underlying database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    int SaveChanges();

    /// <summary>
    ///     Asynchronously saves all changes made in the unit of work to the underlying database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous save operation. The task result contains the number of state entries
    ///     written to the database.
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Begins a new database transaction.
    /// </summary>
    /// <returns>An object that represents the new database transaction.</returns>
    IDbContextTransaction BeginTransaction();

    /// <summary>
    ///     Asynchronously begins a new database transaction.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous transaction operation. The task result contains an object that
    ///     represents the new database transaction.
    /// </returns>
    Task<IDbContextTransaction> BeginTransactionAsync();
}