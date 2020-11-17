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
        private const int BlacklistDatabase = 1;
        private const int WhitelistDatabase = 2;

        private readonly CriThinkRedisMultiplexer _multiplexer;

        public NewsSourceRepository(CriThinkRedisMultiplexer multiplexer)
        {
            _multiplexer = multiplexer ?? throw new ArgumentNullException(nameof(multiplexer));
        }

        public Task<bool> AddNewsSourceToBlacklistAsync(Uri uri, NewsSourceAuthenticity authenticity)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = _multiplexer.GetDatabase(BlacklistDatabase);
            return db.StringSetAsync(GetHostName(uri), authenticity.ToString());
        }

        public Task<bool> AddNewsSourceToWhitelistAsync(Uri uri, NewsSourceAuthenticity authenticity)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = _multiplexer.GetDatabase(WhitelistDatabase);

            return db.StringSetAsync(GetHostName(uri), authenticity.ToString());
        }

        public Task RemoveNewsSourceFromBlacklistAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = _multiplexer.GetDatabase(BlacklistDatabase);
            return RemoveNewsSource(db, uri);
        }

        public Task RemoveNewsSourceFromWhitelistAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = _multiplexer.GetDatabase(WhitelistDatabase);
            return RemoveNewsSource(db, uri);
        }

        public async Task<RedisValue> SearchNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var hostName = GetHostName(uri);

            var db = _multiplexer.GetDatabase(WhitelistDatabase);
            var whitelistValue = await db.StringGetAsync(hostName).ConfigureAwait(false);

            if (whitelistValue.IsNull)
            {
                db = _multiplexer.GetDatabase(BlacklistDatabase);
                var blacklistValue = await db.StringGetAsync(hostName).ConfigureAwait(false);
                return blacklistValue;
            }

            return whitelistValue;
        }

        public async Task<IEnumerable<Tuple<RedisKey, RedisValue>>> GetAllSearchNewsSourcesAsync()
        {
            var whitelistTask = Task.Run(GetAllGoodNewsSources);
            var blacklistTask = Task.Run(GetAllBadNewsSources);

            await Task.WhenAll(whitelistTask, blacklistTask).ConfigureAwait(false);

            var whitelistResults = whitelistTask.Result.ToList();
            var blacklistResults = blacklistTask.Result.ToList();

            return whitelistResults.Union(blacklistResults);
        }

        public IEnumerable<Tuple<RedisKey, RedisValue>> GetAllGoodNewsSources()
        {
            var db = _multiplexer.GetDatabase(WhitelistDatabase);
            var server = _multiplexer.GetServer();

            var results = GetNewsSources(db, server);
            return results;
        }

        public IEnumerable<Tuple<RedisKey, RedisValue>> GetAllBadNewsSources()
        {
            var db = _multiplexer.GetDatabase(BlacklistDatabase);
            var server = _multiplexer.GetServer();

            var results = GetNewsSources(db, server);
            return results;
        }

        #region Privates

        private static IEnumerable<Tuple<RedisKey, RedisValue>> GetNewsSources(IDatabase database, IServer server)
        {
            var list = new List<Tuple<RedisKey, RedisValue>>();

            foreach (var key in server.Keys(database.Database, "*"))
            {
                var redisValue = database.StringGet(key);
                var tuple = new Tuple<RedisKey, RedisValue>($"https://{key}", redisValue);
                list.Add(tuple);
            }

            return list;
        }

        private static Task RemoveNewsSource(IDatabase database, Uri uri)
        {
            return database.KeyDeleteAsync(GetHostName(uri));
        }

        private static string GetHostName(Uri uri) => UriHelper.GetHostNameFromUri(uri);

        #endregion
    }
}
