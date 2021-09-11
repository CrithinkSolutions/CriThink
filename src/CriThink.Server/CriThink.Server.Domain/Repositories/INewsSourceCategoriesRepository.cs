using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.Repositories
{
    public interface INewsSourceCategoriesRepository
    {
        Task<string> GetDescriptionByCategoryNameAsync(
            NewsSourceAuthenticity category,
            CancellationToken cancellationToken = default);
    }
}
