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
        /// <param name="newsLink">Link to save</param>
        /// <param name="authenticity">Authenticity rate</param>
        /// <returns>A task</returns>
        Task<bool> AddNewsSourceAsync(string newsLink, NewsSourceAuthenticity authenticity);

        /// <summary>
        /// Remove a news source from the blacklist
        /// </summary>
        /// <param name="newsLink">Link to remove</param>
        /// <returns>A task</returns>
        Task RemoveNewsSourceAsync(string newsLink);

        /// <summary>
        /// Search for the given source (key) in black and white lists
        /// </summary>
        /// <param name="newsLink">Source (key) to search</param>
        /// <returns>The search result</returns>
        Task<RedisValue> SearchNewsSourceAsync(string newsLink);

        /// <summary>
        /// Get all data from the black and white lists
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<RedisKey, RedisValue>> GetAllSearchNewsSources();
    }
}