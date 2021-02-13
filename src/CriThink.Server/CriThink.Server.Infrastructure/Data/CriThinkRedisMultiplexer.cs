using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CriThink.Server.Infrastructure.Data
{
    public class CriThinkRedisMultiplexer
    {
        private readonly string _connectionString;
        private ConnectionMultiplexer _serverConnection;

        public CriThinkRedisMultiplexer(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("CriThinkRedisCacheConnection") ?? throw new ArgumentNullException(nameof(configuration));
            _serverConnection = ConnectionMultiplexer.Connect(_connectionString);
        }

        /// <summary>
        /// Get the Redis cache connection
        /// </summary>
        /// <returns><see cref="ConnectionMultiplexer"/> Redis instance</returns>
        public ConnectionMultiplexer GetConnection() => _serverConnection ??= ConnectionMultiplexer.Connect(_connectionString);

        /// <summary>
        /// Get the Redis database instance
        /// </summary>
        /// <param name="database">Database number</param>
        /// <returns><see cref="IDatabase"/> Redis database</returns>
        public IDatabase GetDatabase(int database) => GetConnection().GetDatabase(database);

        /// <summary>
        /// Get the Redis database server instance
        /// </summary>
        /// <returns><see cref="IServer"/> Redis server</returns>
        public IServer GetServer() => GetConnection().GetServer(_connectionString);
    }
}
