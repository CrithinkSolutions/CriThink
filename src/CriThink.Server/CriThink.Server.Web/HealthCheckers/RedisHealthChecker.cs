using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CriThink.Server.Web.HealthCheckers
{
    public class RedisHealthChecker : IHealthCheck
    {
        private readonly CriThinkRedisMultiplexer _multiplexer;

        public RedisHealthChecker(CriThinkRedisMultiplexer multiplexer)
        {
            _multiplexer = multiplexer ?? throw new ArgumentNullException(nameof(multiplexer));
        }

        /// <summary>
        /// Returns the Redis connection status
        /// </summary>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            bool isHealthy;

            try
            {
                var redis = _multiplexer.GetConnection();
                isHealthy = redis.IsConnected;
            }
            catch (Exception)
            {
                isHealthy = false;
            }

            if (isHealthy)
            {
                return Task.FromResult<HealthCheckResult>(HealthCheckResult.Healthy("Redis is healty."));
            }

            return Task.FromResult<HealthCheckResult>(HealthCheckResult.Unhealthy("Redis is unhealthy."));
        }
    }
}