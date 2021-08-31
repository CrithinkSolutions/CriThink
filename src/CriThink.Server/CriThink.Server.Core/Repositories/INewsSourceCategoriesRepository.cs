using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Repositories
{
    public interface INewsSourceCategoriesRepository
    {
        Task<string> GetDescriptionByCategoryNameAsync(
            NewsSourceAuthenticity category,
            CancellationToken cancellationToken = default);
    }
}
