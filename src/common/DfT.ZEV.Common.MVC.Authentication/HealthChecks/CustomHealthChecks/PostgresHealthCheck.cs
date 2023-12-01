using DfT.ZEV.Common.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DfT.ZEV.Common.MVC.Authentication.HealthChecks.CustomHealthChecks;

internal class PostgresHealthCheck : IHealthCheck
{
    private readonly IOptions<PostgresConfiguration> _postgresConfiguration;

    public PostgresHealthCheck(IOptions<PostgresConfiguration> postgresConfiguration)
    {
        _postgresConfiguration = postgresConfiguration;
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
        catch(Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Connections failed to open to the database.",
                exception: ex);
        }
    }
}