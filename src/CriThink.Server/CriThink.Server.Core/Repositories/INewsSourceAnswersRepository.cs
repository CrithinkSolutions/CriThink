using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface INewsSourceAnswersRepository
    {
        Task<IList<ArticleAnswer>> GetNewsSourceAnswersByUserId(
            Guid userId,
            string newsLink,
            CancellationToken cancellationToken = default);
    }
}
