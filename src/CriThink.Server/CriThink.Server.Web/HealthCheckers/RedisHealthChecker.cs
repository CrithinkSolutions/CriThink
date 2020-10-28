using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class RedisHealthChecker : IHealthCheck
{
    /// <summary>
    /// Returns the Redis connection status
    /// </summary>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        bool isHealthy;

        try
        {
            var redis = CriThinkRedisMultiplexer.GetConnection();
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