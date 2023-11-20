using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Zev.Core.Infrastructure.Persistence;

public class TestableAppDbContext : AppDbContext
{
    public virtual DatabaseFacade TestableDatabase => base.Database;

    public virtual IDbContextTransaction BeginTransaction()
    {
        return TestableDatabase.BeginTransaction();
    }

    public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return TestableDatabase.BeginTransactionAsync(cancellationToken);
    }
}