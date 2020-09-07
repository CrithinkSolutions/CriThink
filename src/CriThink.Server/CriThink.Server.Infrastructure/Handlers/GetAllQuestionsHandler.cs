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
    internal class GetAllQuestionsHandler : IRequestHandler<GetAllQuestionsQuery, List<Question>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllQuestionsHandler> _logger;

        public GetAllQuestionsHandler(CriThinkDbContext dbContext, ILogger<GetAllQuestionsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<List<Question>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var allQuestions = await _dbContext.Questions
                    .GetAllQuestionsAsync(QuestionProjection.GetAll, cancellationToken)
                    .ConfigureAwait(false);

                return allQuestions;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all question", request);
                throw;
            }
        }
    }
}
