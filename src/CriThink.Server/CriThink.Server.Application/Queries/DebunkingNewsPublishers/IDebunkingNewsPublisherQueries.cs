using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Application.Queries
{
    public interface IDebunkingNewsPublisherQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publisherName"></param>
        /// <returns></returns>
        Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(string publisherName);
    }
}
