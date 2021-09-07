using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface INewsSourceAnswersRepository : IRepository<NewsSoucePostAnswer>
    {
        Task<IList<NewsSoucePostAnswer>> GetNewsSourceAnswersByUserId(
            Guid userId,
            string newsLink,
            CancellationToken cancellationToken = default);

        Task<IList<NewsSoucePostAnswer>> GetNewsSourceAnswersByNewsLinkAsync(
            string newsLink,
            CancellationToken cancellationToken = default);

        Task AddAsync(NewsSoucePostAnswer answer);
    }
}
