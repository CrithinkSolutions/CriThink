using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using StackExchange.Redis;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal interface INewsSourceRepository
    {
        /// <summary>
        /// Adds the given source with the authenticity rate to a blacklist
        /// </summary>
        /// <param name="uri">Host to save</param>
        /// <param name="authenticity">Authenticity rate</param>
        /// <returns>A task</returns>
        Task<bool> AddNewsSourceToBlacklistAsync(Uri uri, NewsSourceAuthenticity authenticity);

        /// <summary>
        /// Adds the given source with the authenticity rate to a whitelist
        /// </summary>
        /// <param name="uri">Host to save</param>
        /// <param name="authenticity">Authenticity rate</param>
        /// <returns>A task</returns>
        Task<bool> AddNewsSourceToWhitelistAsync(Uri uri, NewsSourceAuthenticity authenticity);

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