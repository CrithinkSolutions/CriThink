using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.Repositories
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
