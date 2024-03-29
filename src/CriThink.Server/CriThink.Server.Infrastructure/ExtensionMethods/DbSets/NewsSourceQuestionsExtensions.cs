﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class NewsSourceQuestionsExtensions
    {
        /// <summary>
        /// Get news source questions by category
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="questionCategory">News source category</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<NewsSourcePostQuestion>> GetNewsSourceQuestionsByCategoryAsync(
            this DbSet<NewsSourcePostQuestion> dbSet,
            QuestionCategory questionCategory,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                   .Where(aq => aq.Category == questionCategory)
                   .AsNoTracking()
                   .ToListAsync(cancellationToken);
        }
    }
}
