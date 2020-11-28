﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class QuestionExtensions
    {
        /// <summary>
        /// Get all the demo news
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        internal static Task<List<Question>> GetAllQuestionsAsync(this DbSet<Question> dbSet, Expression<Func<Question, Question>> projection, CancellationToken cancellationToken = default)
        {
            return dbSet
                .Select(projection)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get question with the given id
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="questionId">Question id to search</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns></returns>
        internal static ValueTask<Question> GetQuestionByIdAsync(this DbSet<Question> dbSet, Guid questionId, CancellationToken cancellationToken = default)
        {
            return dbSet
                .FindAsync(new object[] { questionId }, cancellationToken);
        }
    }
}
