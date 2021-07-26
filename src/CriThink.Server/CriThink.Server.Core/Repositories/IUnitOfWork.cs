using System;
using System.Threading;
using System.Threading.Tasks;

namespace CriThink.Server.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
