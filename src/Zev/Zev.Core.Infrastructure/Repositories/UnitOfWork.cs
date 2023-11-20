using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context;
    private bool _disposed;
    private readonly ILogger _logger;
    public IVehicleRepository Vehicles { get; }

    public UnitOfWork(AppDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        Vehicles = new VehicleRepository(_context, logger);
    }

    
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    protected void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}