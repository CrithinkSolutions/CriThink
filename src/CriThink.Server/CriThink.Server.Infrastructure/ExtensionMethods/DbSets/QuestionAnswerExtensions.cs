using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class QuestionAnswerExtensions
    {
        /// <summary>
        /// Get all the answers
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="newsId">News id</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        internal static Task<List<QuestionAnswer>> GetQuestionAnswersOfGivenNewsAsync(this DbSet<QuestionAnswer> dbSet, Expression<Func<QuestionAnswer, QuestionAnswer>> projection, Guid newsId, IReadOnlyList<Guid> questionIds, CancellationToken cancellationToken = default)
        {
            return dbSet
                .Include(qa => qa.DemoNews)
                .Include(qa => qa.Question)
                .Where(qa => qa.DemoNews.Id == newsId && questionIds.Contains(qa.Question.Id))
                .Select(projection)
                .ToListAsync(cancellationToken);
        }
    }
}
