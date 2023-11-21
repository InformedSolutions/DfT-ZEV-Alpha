using Microsoft.EntityFrameworkCore.Storage;
using Zev.Core.Domain.Vehicles;

namespace Zev.Core.Infrastructure.Repositories;

public interface IUnitOfWork
{
    IVehicleRepository Vehicles { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync();
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
}