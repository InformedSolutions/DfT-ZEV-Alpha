using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Services;
using Microsoft.EntityFrameworkCore.Storage;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using DfT.ZEV.Core.Infrastructure.Repositories;

namespace DfT.ZEV.Core.Infrastructure.UnitOfWork;

internal sealed class UnitOfWork : IUnitOfWork
{
    public IVehicleRepository Vehicles => _vehicles ??= new VehicleRepository(_dbContext);
    public IProcessRepository Processes => _processes ??= new ProcessRepository(_dbContext);
    public IUserRepository Users => _users ??= new UserRepository(_dbContext);

    private readonly AppDbContext _dbContext;
    private IVehicleRepository _vehicles;
    private IProcessRepository _processes;
    private IUserRepository _users;
    private IDbContextTransaction _transaction;
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new Exception("Transaction already started.");
        
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return _transaction.TransactionId;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is null)
            throw new Exception("Transaction not started.");
        
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }   

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is null)
            throw new Exception("Transaction not started.");
        
        await _transaction.RollbackAsync(cancellationToken);
    }
}