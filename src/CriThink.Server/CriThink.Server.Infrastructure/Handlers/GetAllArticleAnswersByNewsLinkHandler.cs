using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class GetAllArticleAnswersByNewsLinkHandler : IRequestHandler<GetAllArticleAnswersByNewsLinkQuery, IList<ArticleAnswer>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllArticleAnswersByNewsLinkHandler> _logger;

        public GetAllArticleAnswersByNewsLinkHandler(CriThinkDbContext dbContext, ILogger<GetAllArticleAnswersByNewsLinkHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<IList<ArticleAnswer>> Handle(GetAllArticleAnswersByNewsLinkQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var answers = await _dbContext
                    .ArticleAnswers
                    .Where(aa => aa.NewsLink == request.NewsLink)
                    .ToListAsync(cancellationToken);

                return answers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting article answers by news link");
                throw;
            }
        }
    }
}
