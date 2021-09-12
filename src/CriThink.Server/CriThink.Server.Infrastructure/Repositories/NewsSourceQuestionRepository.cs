﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class NewsSourceQuestionRepository : INewsSourceQuestionRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public NewsSourceQuestionRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<NewsSourcePostQuestion>> GetQuestionsByCategoryAsync(
            QuestionCategory category,
            CancellationToken cancellationToken = default)
        {
            var questions = await _dbContext
                .NewsSourcePostQuestions.GetNewsSourceQuestionsByCategoryAsync(category, cancellationToken);

            return questions;
        }
    }
}