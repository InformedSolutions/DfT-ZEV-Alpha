using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Domain.Manufacturers.Services;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;

namespace DfT.ZEV.Core.Domain.Abstractions;

public interface IUnitOfWork
{
    IVehicleRepository Vehicles { get; }
    IProcessRepository Processes { get; }
    IUserRepository Users { get; }
    IManufacturerRepository Manufacturers { get; }
    IPermissionRepository Permissions { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}