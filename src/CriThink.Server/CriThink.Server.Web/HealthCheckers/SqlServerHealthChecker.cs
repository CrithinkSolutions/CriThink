using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class SqlServerHealthChecker : IHealthCheck
{
    private readonly CriThinkDbContext _dbContext;

    public SqlServerHealthChecker(CriThinkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        bool isHealthy = false;

        try
        {
            _dbContext.Users.First();

            isHealthy = true;
        }
        catch (Exception)
        {
            isHealthy = false;
        }

        if (isHealthy)
        {
            return await Task.FromResult<HealthCheckResult>(HealthCheckResult.Healthy("SQL Server is running")).ConfigureAwait(false);
        }

        return await Task.FromResult<HealthCheckResult>(HealthCheckResult.Unhealthy("SQL Server is unhealthy.")).ConfigureAwait(false);
    }
}