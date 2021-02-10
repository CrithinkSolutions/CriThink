using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using StackExchange.Redis;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class NewsSourceRepository : INewsSourceRepository
    {
        private const int NewsSourceDatabase = 1;

        private readonly CriThinkRedisMultiplexer _multiplexer;

        public NewsSourceRepository(CriThinkRedisMultiplexer multiplexer)
        {
            _multiplexer = multiplexer ?? throw new ArgumentNullException(nameof(multiplexer));
        }

        public Task<bool> AddNewsSourceAsync(Uri uri, NewsSourceAuthenticity authenticity)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            return db.StringSetAsync(GetHostName(uri), authenticity.ToString());
        }

        public Task RemoveNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            return RemoveNewsSource(db, uri);
        }

        public async Task<RedisValue> SearchNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var hostName = GetHostName(uri);

            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            var whitelistValue = await db.StringGetAsync(hostName).ConfigureAwait(false);
            return whitelistValue;
        }

        public IEnumerable<Tuple<RedisKey, RedisValue>> GetAllSearchNewsSources()
        {
            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            var server = _multiplexer.GetServer();

            var results = GetNewsSources(db, server);
            return results;
        }

        #region Privates

        private static IEnumerable<Tuple<RedisKey, RedisValue>> GetNewsSources(IDatabase database, IServer server)
        {
            return server
                .Keys(database.Database, "*")
                .Select(k => ReadValue(k, database));
        }

        private static Tuple<RedisKey, RedisValue> ReadValue(RedisKey key, IDatabase database)
        {
            var redisValue = database.StringGet(key);
            var tuple = new Tuple<RedisKey, RedisValue>($"https://{key}", redisValue);
            return tuple;
        }

        private static Task RemoveNewsSource(IDatabase database, Uri uri)
        {
            return database.KeyDeleteAsync(GetHostName(uri));
        }

        private static string GetHostName(Uri uri) => UriHelper.GetHostNameFromUri(uri);

        #endregion
    }
}
