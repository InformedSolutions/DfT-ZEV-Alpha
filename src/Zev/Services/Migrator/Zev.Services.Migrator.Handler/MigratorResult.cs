using System.Collections.Generic;

namespace Zev.Services.Migrator.Handler;

public class MigratorResult
{
    public IEnumerable<string> MigrationsAlreadyApplied { get; set; }

    public IEnumerable<string> MigrationsAppliedInCurrentRun { get; set; }
}