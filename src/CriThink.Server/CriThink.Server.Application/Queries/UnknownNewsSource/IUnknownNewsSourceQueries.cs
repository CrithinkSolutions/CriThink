using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Application.Queries
{
    public interface IUnknownNewsSourceQueries
    {
        Task<UnknownNewsSource> GetUnknownNewsSourceByUriAsync(string newsSourceLink, CancellationToken cancellationToken);
    }
}
