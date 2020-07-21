using StackExchange.Redis;

namespace CriThink.Server.Infrastructure.Data
{
    public static class CriThinkRedisMultiplexer
    {
        private const string ConnectionString = "127.0.0.1:6379";

        private static ConnectionMultiplexer _serverConnection;

        /// <summary>
        /// Get the Redis cache connection
        /// </summary>
        /// <returns><see cref="ConnectionMultiplexer"/> Redis instance</returns>
        public static ConnectionMultiplexer GetConnection() => _serverConnection ??= ConnectionMultiplexer.Connect(ConnectionString);

        /// <summary>
        /// Get the Redis database instance
        /// </summary>
        /// <param name="database">Database number</param>
        /// <returns><see cref="IDatabase"/> Redis database</returns>
        public static IDatabase GetDatabase(int database) => GetConnection().GetDatabase(database);

        /// <summary>
        /// Get the Redis database server instance
        /// </summary>
        /// <returns><see cref="IServer"/> Redis server</returns>
        public static IServer GetServer() => GetConnection().GetServer(ConnectionString);
    }
}
