using DfT.ZEV.Common.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DfT.ZEV.Common.MVC.Authentication.HealthChecks.CustomHealthChecks;

public class PostgresHealthCheck : IHealthCheck
{
    private readonly IOptions<PostgresConfiguration> _postgresConfiguration;
    private readonly ILogger<PostgresHealthCheck> _logger;
    public PostgresHealthCheck(IOptions<PostgresConfiguration> postgresConfiguration, ILogger<PostgresHealthCheck> logger)
    {
        _postgresConfiguration = postgresConfiguration;
        _logger = logger;
    }

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