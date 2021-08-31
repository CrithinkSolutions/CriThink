using System.Threading;
using System.Threading.Tasks;

namespace CriThink.Server.Core.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task DeleteExpiredTokensAsync(CancellationToken cancellationToken = default);
    }
}
