using Microsoft.EntityFrameworkCore.Storage;
using Zev.Core.Domain.Processes.Services;
using Zev.Core.Domain.Vehicles.Services;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

/// <inheritdoc cref="Zev.Core.Infrastructure.Repositories.IUnitOfWork" />
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
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IVehicleRepository Vehicles { get; }
    public IProcessRepository Processes { get; }

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