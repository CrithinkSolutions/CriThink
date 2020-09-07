using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class GetAllQuestionAnswerHandler : IRequestHandler<GetAllQuestionAnswerQuery, List<QuestionAnswer>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllQuestionAnswerHandler> _logger;

        public GetAllQuestionAnswerHandler(CriThinkDbContext dbContext, ILogger<GetAllQuestionAnswerHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<List<QuestionAnswer>> Handle(GetAllQuestionAnswerQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var newsId = request.NewsId;
                var questionIds = request.QuestionIds;

                return await _dbContext.QuestionAnswers
                    .GetQuestionAnswersOfGivenNewsAsync(QuestionAnswerProjection.GetAll, newsId, questionIds, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all question answers", request);
                throw;
            }
        }
    }
}
