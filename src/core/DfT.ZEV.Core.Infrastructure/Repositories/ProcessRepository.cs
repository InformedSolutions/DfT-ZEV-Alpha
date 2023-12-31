using Microsoft.EntityFrameworkCore;
using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

/// <inheritdoc/>
internal sealed class ProcessRepository : IProcessRepository
{
    private readonly AppDbContext _context;

    public ProcessRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Process?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Processes
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);


    /// <inheritdoc/>
    public async Task<IEnumerable<Process>> GetPagedAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
        => await _context.Processes
                .AsNoTracking()
                .OrderByDescending(x => x.LastUpdated)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);


    /// <inheritdoc/>
    public async Task AddAsync(Process process, CancellationToken cancellationToken = default)
        => await _context.Processes.AddAsync(process, cancellationToken);


    /// <inheritdoc/>
    public void Update(Process process)
        => _context.Processes.Update(process);


    /// <inheritdoc/>
    public void Delete(Process process)
        => _context.Processes.Remove(process);

}