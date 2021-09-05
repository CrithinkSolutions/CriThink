using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using StackExchange.Redis;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class NewsSourceRepository : INewsSourceRepository
    {
        private const int NewsSourceDatabase = 1;

        private readonly CriThinkRedisMultiplexer _multiplexer;

        public NewsSourceRepository(
            CriThinkRedisMultiplexer multiplexer)
        {
            _multiplexer = multiplexer ??
                throw new ArgumentNullException(nameof(multiplexer));
        }

        public Task<bool> AddNewsSourceAsync(
            string newsLink,
            NewsSourceAuthenticity authenticity)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            return db.StringSetAsync(newsLink, authenticity.ToString());
        }

        public Task RemoveNewsSourceAsync(string newsLink)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            return RemoveNewsSource(db, newsLink);
        }

        public async Task<NewsSource> SearchNewsSourceAsync(string newsLink)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            var result = await db.StringGetAsync(newsLink);
            if (result.IsNull)
                return null;

            return new NewsSource(
                newsLink,
                result);
        }

        public IEnumerable<NewsSource> GetAllSearchNewsSources()
        {
            var db = _multiplexer.GetDatabase(NewsSourceDatabase);
            var server = _multiplexer.GetServer();

            var results = GetNewsSources(db, server);
            return results;
        }

        #region Privates

        private static IEnumerable<NewsSource> GetNewsSources(IDatabase database, IServer server)
        {
            return server
                .Keys(database.Database, "*")
                .Select(k => ReadValue(k, database));
        }

        private static NewsSource ReadValue(RedisKey key, IDatabase database)
        {
            var redisValue = database.StringGet(key);
            return new NewsSource(
                key,
                redisValue);
        }

        private static Task RemoveNewsSource(IDatabase database, string newsLink)
        {
            return database.KeyDeleteAsync(newsLink);
        }

        #endregion
    }
}
