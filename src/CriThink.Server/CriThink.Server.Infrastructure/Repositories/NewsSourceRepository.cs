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
    public class NewsSourceRepository : INewsSourceRepository
    {
        private const int BlacklistDatabase = 1;
        private const int WhitelistDatabase = 2;

        public Task<bool> AddNewsSourceToBlacklistAsync(Uri uri, NewsSourceAuthencity authencity)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = CriThinkRedisMultiplexer.GetDatabase(BlacklistDatabase);
            return db.StringSetAsync(GetHostName(uri), authencity.ToString());
        }

        public Task<bool> AddNewsSourceToWhitelistAsync(Uri uri, NewsSourceAuthencity authencity)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = CriThinkRedisMultiplexer.GetDatabase(WhitelistDatabase);

            return db.StringSetAsync(GetHostName(uri), authencity.ToString());
        }

        public Task RemoveNewsSourceFromBlacklistAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = CriThinkRedisMultiplexer.GetDatabase(BlacklistDatabase);
            return RemoveNewsSource(db, uri);
        }

        public Task RemoveNewsSourceFromWhitelistAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var db = CriThinkRedisMultiplexer.GetDatabase(WhitelistDatabase);
            return RemoveNewsSource(db, uri);
        }

        public async Task<RedisValue> SearchNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var hostName = GetHostName(uri);

            var db = CriThinkRedisMultiplexer.GetDatabase(WhitelistDatabase);
            var whitelistValue = await db.StringGetAsync(hostName).ConfigureAwait(false);

            if (whitelistValue.IsNull)
            {
                db = CriThinkRedisMultiplexer.GetDatabase(BlacklistDatabase);
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
            var db = CriThinkRedisMultiplexer.GetDatabase(WhitelistDatabase);
            var server = CriThinkRedisMultiplexer.GetServer();

            var results = GetNewsSources(db, server);
            return results;
        }

        public IEnumerable<Tuple<RedisKey, RedisValue>> GetAllBadNewsSources()
        {
            var db = CriThinkRedisMultiplexer.GetDatabase(BlacklistDatabase);
            var server = CriThinkRedisMultiplexer.GetServer();

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

    public interface INewsSourceRepository
    {
        /// <summary>
        /// Adds the given source with the authenticity rate to a blacklist
        /// </summary>
        /// <param name="uri">Host to save</param>
        /// <param name="authencity">Authenticity rate</param>
        /// <returns>A task</returns>
        Task<bool> AddNewsSourceToBlacklistAsync(Uri uri, NewsSourceAuthencity authencity);

        /// <summary>
        /// Adds the given source with the authenticity rate to a whitelist
        /// </summary>
        /// <param name="uri">Host to save</param>
        /// <param name="authencity">Authenticity rate</param>
        /// <returns>A task</returns>
        Task<bool> AddNewsSourceToWhitelistAsync(Uri uri, NewsSourceAuthencity authencity);

        /// <summary>
        /// Remove a news source from the blacklist
        /// </summary>
        /// <param name="uri">Host to remove</param>
        /// <returns>A task</returns>
        Task RemoveNewsSourceFromBlacklistAsync(Uri uri);

        /// <summary>
        /// Remove a news source from the whitelist
        /// </summary>
        /// <param name="uri">Host to remove</param>
        /// <returns>A task</returns>
        Task RemoveNewsSourceFromWhitelistAsync(Uri uri);

        /// <summary>
        /// Search for the given source (key) in black and white lists
        /// </summary>
        /// <param name="uri">Source (key) to search</param>
        /// <returns>The search result</returns>
        Task<RedisValue> SearchNewsSourceAsync(Uri uri);

        /// <summary>
        /// Get all data from the black and white lists
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tuple<RedisKey, RedisValue>>> GetAllSearchNewsSourcesAsync();

        /// <summary>
        /// Get all data from the whitelist
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<RedisKey, RedisValue>> GetAllGoodNewsSources();

        /// <summary>
        /// Get all data from the blacklist
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<RedisKey, RedisValue>> GetAllBadNewsSources();
    }
}
