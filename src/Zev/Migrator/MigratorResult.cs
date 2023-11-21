using System.Collections.Generic;

namespace Migrator;

public class MigratorResult
{
    public IEnumerable<string> MigrationsAlreadyApplied { get; set; }

    public IEnumerable<string> MigrationsAppliedInCurrentRun { get; set; }
}
