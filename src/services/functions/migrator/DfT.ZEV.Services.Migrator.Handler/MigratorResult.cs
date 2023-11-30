using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DfT.ZEV.Services.Migrator.Handler;

public class MigratorResult
{
    public IEnumerable<string> MigrationsAlreadyApplied { get; set; }

    public IEnumerable<string> MigrationsAppliedInCurrentRun { get; set; }
}