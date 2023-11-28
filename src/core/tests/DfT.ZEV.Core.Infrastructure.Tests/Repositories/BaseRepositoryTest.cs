using AutoFixture;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Tests.Repositories;

public class BaseRepositoryTest<TRepository>
    where TRepository : class
{
    public AppDbContext _context = null!;
    public IFixture _fixture = null!;
    public TRepository _repository = null!;

    [SetUp]
    public void SetUp()
    {
        var options = InMemoryOptions();
        _context = (AppDbContext)Activator.CreateInstance(typeof(AppDbContext), options)!;
        _repository = (TRepository)Activator.CreateInstance(typeof(TRepository), _context)!;
        _fixture = new Fixture();
    }

    private DbContextOptions<AppDbContext> LocalPostgresOptions()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=zev-dev;Username=root;Password=root;Maximum Pool Size=10;SSL Mode=Disable;Trust Server Certificate=true")
            .Options;

        return options;
    }

    private DbContextOptions<AppDbContext> InMemoryOptions()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return options;
    }
}