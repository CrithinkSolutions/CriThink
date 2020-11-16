using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CriThink.Server.Infrastructure.Data
{
    //public static class CriThinkRedisMultiplexer
    //{
    //    private static string _connectionString;

    //    private static ConnectionMultiplexer _serverConnection;

    //    /// <summary>
    //    /// Get the Redis cache connection
    //    /// </summary>
    //    /// <returns><see cref="ConnectionMultiplexer"/> Redis instance</returns>
    //    public static ConnectionMultiplexer GetConnection() => _serverConnection ??= ConnectionMultiplexer.Connect(_connectionString);

    //    /// <summary>
    //    /// Get the Redis database instance
    //    /// </summary>
    //    /// <param name="database">Database number</param>
    //    /// <returns><see cref="IDatabase"/> Redis database</returns>
    //    public static IDatabase GetDatabase(int database) => GetConnection().GetDatabase(database);

    //    /// <summary>
    //    /// Get the Redis database server instance
    //    /// </summary>
    //    /// <returns><see cref="IServer"/> Redis server</returns>
    //    public static IServer GetServer() => GetConnection().GetServer(_connectionString);

    //    /// <summary>
    //    /// Set the Redis configuration
    //    /// </summary>
    //    /// <param name="configuration">The string configuration</param>
    //    internal static void SetRedisConfiguration(string configuration)
    //    {
    //        _connectionString = configuration;
    //        _serverConnection = null;
    //    }
    //}

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
