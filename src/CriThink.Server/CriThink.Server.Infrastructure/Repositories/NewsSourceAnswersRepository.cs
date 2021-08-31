using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class NewsSourceAnswersRepository : INewsSourceAnswersRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public NewsSourceAnswersRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<ArticleAnswer>> GetNewsSourceAnswersByUserId(
            Guid userId,
            string newsLink,
            CancellationToken cancellationToken = default)
        {
            var answers = await _dbContext
                    .ArticleAnswers
                    .GetNewsSourceAnswersByUserIdAsync(userId, newsLink, cancellationToken);

            return answers;
        }
    }
}
