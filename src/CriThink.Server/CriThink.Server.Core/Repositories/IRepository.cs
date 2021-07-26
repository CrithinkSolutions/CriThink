using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
