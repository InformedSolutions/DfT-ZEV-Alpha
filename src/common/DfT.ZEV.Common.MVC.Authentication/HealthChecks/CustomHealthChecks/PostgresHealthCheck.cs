using DfT.ZEV.Common.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DfT.ZEV.Common.MVC.Authentication.HealthChecks.CustomHealthChecks;

/// <summary>
/// Represents a health check for a PostgreSQL database.
/// </summary>
public class PostgresHealthCheck : IHealthCheck
{
    private readonly IOptions<PostgresConfiguration> _postgresConfiguration;
    private readonly ILogger<PostgresHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresHealthCheck"/> class.
    /// </summary>
    /// <param name="postgresConfiguration">The PostgreSQL configuration.</param>
    /// <param name="logger">The logger.</param>
    public PostgresHealthCheck(IOptions<PostgresConfiguration> postgresConfiguration, ILogger<PostgresHealthCheck> logger)
    {
        _postgresConfiguration = postgresConfiguration;
        _logger = logger;
    }

    /// <summary>
    /// Checks the health of the PostgreSQL database.
    /// </summary>
    /// <param name="context">A context object associated with the current execution.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
    /// <returns>A <see cref="Task"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            await using var sqlConnection = new NpgsqlConnection(_postgresConfiguration.Value.ConnectionString);

            await sqlConnection.OpenAsync(cancellationToken);

            await using var command = sqlConnection.CreateCommand();
            command.CommandText = "SELECT 1";

            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy("Successfully connected to the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while checking the database health.");
            return HealthCheckResult.Unhealthy(
                "Cannot connect to the database."
                );
        }
    }
}