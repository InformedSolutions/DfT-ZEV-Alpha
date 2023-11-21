using Microsoft.EntityFrameworkCore.Storage;
using Zev.Core.Domain.Processes.Services;
using Zev.Core.Domain.Vehicles;

namespace Zev.Core.Infrastructure.Repositories;

public interface IUnitOfWork
{
    IVehicleRepository Vehicles { get; }
    IProcessRepository Processes { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync();
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
}