using DfT.ZEV.Core.Domain.Accounts.Services;
using Microsoft.EntityFrameworkCore.Storage;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Vehicles.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

/// <inheritdoc cref="DfT.ZEV.Core.Infrastructure.Repositories.IUnitOfWork" />
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context;
    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UnitOfWork" /> class.
    /// </summary>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Vehicles = new VehicleRepository(_context);
        Processes = new ProcessRepository(_context);
        Users = new UserRepository(_context);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IVehicleRepository Vehicles { get; }
    public IProcessRepository Processes { get; }
    public IUserRepository Users { get; }

    /// <inheritdoc />
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    /// <inheritdoc />
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    protected void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }
}