using DfT.ZEV.Core.Domain.Manufacturers.Models;
using DfT.ZEV.Core.Domain.Manufacturers.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

/// <inheritdoc/>
internal sealed class ManufacturerRepository : IManufacturerRepository
{
    private readonly AppDbContext _context;

    public ManufacturerRepository(AppDbContext context) => _context = context;

    /// <inheritdoc/>
    public async ValueTask<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async ValueTask<Manufacturer?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

    /// <inheritdoc/>
    public async ValueTask<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Manufacturers.ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async ValueTask<IEnumerable<Manufacturer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async ValueTask<IEnumerable<string>> GetManufacturerNamesAsync(CancellationToken cancellationToken = default)
        => await _context.Manufacturers.Select(x => x.Name).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task InsertAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.AddAsync(manufacturer,cancellationToken);

    /// <inheritdoc/>
    public async Task BulkInsertAsync(IList<Manufacturer> manufacturers, CancellationToken cancellationToken = default)
        => await _context.BulkInsertAsync(manufacturers, cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public void Update(Manufacturer manufacturer)
        => _context.Manufacturers.Update(manufacturer);

    /// <inheritdoc/>
    public void Delete(Manufacturer manufacturer)
        => _context.Manufacturers.Remove(manufacturer);
}