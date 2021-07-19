using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class GetAllQuestionsHandler : IRequestHandler<GetAllQuestionsQuery, GetAllArticleQuestionsResponse>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllQuestionsHandler> _logger;

        public GetAllQuestionsHandler(CriThinkDbContext dbContext, ILogger<GetAllQuestionsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<GetAllArticleQuestionsResponse> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var questions = await _dbContext
                    .ArticleQuestions
                    .ToListAsync();

                return new GetAllArticleQuestionsResponse(questions);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting list of article questions");
                throw;
            }
        }
    }
}
