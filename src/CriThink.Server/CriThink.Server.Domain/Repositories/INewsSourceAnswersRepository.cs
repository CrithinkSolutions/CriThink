using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.Repositories
{
    public interface INewsSourceAnswersRepository : IRepository<NewsSourcePostAnswer>
    {
        Task<IList<NewsSourcePostAnswer>> GetNewsSourceAnswersByUserId(
            Guid userId,
            string newsLink,
            CancellationToken cancellationToken = default);

        Task<IList<NewsSourcePostAnswer>> GetNewsSourceAnswersByNewsLinkAsync(
            string newsLink,
            CancellationToken cancellationToken = default);

        Task AddAsync(NewsSourcePostAnswer answer);
    }
}
