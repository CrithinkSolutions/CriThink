using System;
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
    internal class GetQuestionHandler : IRequestHandler<GetQuestionQuery, Question>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetQuestionHandler> _logger;

        public GetQuestionHandler(CriThinkDbContext dbContext, ILogger<GetQuestionHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Question> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var allQuestions = await _dbContext.Questions
                    .GetQuestionByIdAsync(QuestionProjection.GetAll, request.QuestionId, cancellationToken)
                    .ConfigureAwait(false);

                return allQuestions;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting single question", request);
                throw;
            }
        }
    }
}
