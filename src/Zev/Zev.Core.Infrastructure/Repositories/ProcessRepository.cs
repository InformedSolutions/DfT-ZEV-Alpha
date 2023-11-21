using Microsoft.EntityFrameworkCore;
using Zev.Core.Domain.Processes.Models;
using Zev.Core.Domain.Processes.Services;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

public class ProcessRepository : IProcessRepository
{
    private readonly AppDbContext _context;

    public ProcessRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Process?> GetProcessByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Processes.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddProcessAsync(Process process, CancellationToken ct = default) 
        => await _context.Processes.AddAsync(process, ct);

    public async Task UpdateProcessAsync(Process process, CancellationToken ct = default)
        => _context.Update(process);

    public async Task<IEnumerable<Process>> GetProcessesAsync(CancellationToken ct = default)
        => await _context.Processes.ToListAsync(ct);
}