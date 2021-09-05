using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface INewsSourceRepository
    {
        Task<bool> AddNewsSourceAsync(
            string newsLink,
            NewsSourceAuthenticity authenticity);

        Task RemoveNewsSourceAsync(
            string newsLink);

        Task<NewsSource> SearchNewsSourceAsync(
            string newsLink);

        IEnumerable<NewsSource> GetAllSearchNewsSources();
    }
}
