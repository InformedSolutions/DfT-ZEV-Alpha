using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using Zev.Core.Domain.Processes.Services;
using Zev.Core.Domain.Vehicles.Services;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

/// <inheritdoc cref="Zev.Core.Infrastructure.Repositories.IUnitOfWork" />
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UnitOfWork" /> class.
    /// </summary>
    public UnitOfWork(AppDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        Vehicles = new VehicleRepository(_context, logger);
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
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
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