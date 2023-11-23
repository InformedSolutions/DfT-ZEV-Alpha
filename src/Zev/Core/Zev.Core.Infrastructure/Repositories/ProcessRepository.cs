using Microsoft.EntityFrameworkCore;
using Zev.Core.Domain.Processes.Models;
using Zev.Core.Domain.Processes.Services;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

public sealed class ProcessRepository : IProcessRepository
{
    private readonly AppDbContext _context;

    public ProcessRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Process?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Processes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Process>> GetPagedAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.Processes.OrderByDescending(x => x.LastUpdated).Skip(page * pageSize).Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Process process, CancellationToken cancellationToken = default)
    {
        await _context.Processes.AddAsync(process, cancellationToken);
    }

    public void Update(Process process)
    {
        _context.Processes.Update(process);
    }

    public void Delete(Process process)
    {
        _context.Processes.Remove(process);
    }
}