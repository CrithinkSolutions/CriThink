using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CriThink.Server.Web.HealthCheckers
{
    public class PostgreSqlHealthChecker : IHealthCheck
    {
        private readonly CriThinkDbContext _dbContext;

        public PostgreSqlHealthChecker(CriThinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            bool isHealthy = false;

            try
            {
                _ = _dbContext.Users.First();

                isHealthy = true;
            }
            catch (Exception)
            {
                isHealthy = false;
            }

            if (isHealthy)
            {
                return await Task.FromResult(HealthCheckResult.Healthy("PostgreSQL is running"))
                                 .ConfigureAwait(false);
            }

            return await Task.FromResult(HealthCheckResult.Unhealthy("PostgreSQL is unhealthy."))
                             .ConfigureAwait(false);
        }
    }
}