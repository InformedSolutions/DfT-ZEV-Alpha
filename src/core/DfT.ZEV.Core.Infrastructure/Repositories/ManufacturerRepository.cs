using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

internal sealed class ManufacturerRepository : IManufacturerRepository
{
    private readonly AppDbContext _context;

    public ManufacturerRepository(AppDbContext context) => _context = context;

    public async ValueTask<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async ValueTask<Manufacturer?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

    public async ValueTask<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Manufacturers.ToListAsync(cancellationToken);

    public async ValueTask<IEnumerable<Manufacturer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);

    public async Task InsertAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default)
        => await _context.Manufacturers.AddAsync(manufacturer,cancellationToken);

    public void Update(Manufacturer manufacturer)
        => _context.Manufacturers.Update(manufacturer);

    public void Delete(Manufacturer manufacturer)
        => _context.Manufacturers.Remove(manufacturer);
}