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
    internal class GetAllArticleAnswersByUserIdAndNewsLinkHandler : IRequestHandler<GetAllArticleAnswersByUserIdAndNewsLinkQuery, IList<ArticleAnswer>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllArticleAnswersByUserIdAndNewsLinkHandler> _logger;

        public GetAllArticleAnswersByUserIdAndNewsLinkHandler(CriThinkDbContext dbContext, ILogger<GetAllArticleAnswersByUserIdAndNewsLinkHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<IList<ArticleAnswer>> Handle(GetAllArticleAnswersByUserIdAndNewsLinkQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var userId = Guid.Parse(request.UserId);

                var answers = await _dbContext
                    .ArticleAnswers
                    .Where(aa => aa.Id == userId && aa.NewsLink == request.NewsLink)
                    .ToListAsync(cancellationToken);

                return answers;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting article anwsers by user id and news link");
                throw;
            }
        }
    }
}
