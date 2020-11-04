using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class GetDebunkingNewsHandler : IRequestHandler<GetDebunkingNewsQuery, DebunkingNews>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetDebunkingNewsHandler> _logger;

        public GetDebunkingNewsHandler(CriThinkDbContext dbContext, ILogger<GetDebunkingNewsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<DebunkingNews> Handle(GetDebunkingNewsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var debunkingNews = await _dbContext.DebunkingNews
                    .GetDebunkingNewsAsync(request.Id, cancellationToken)
                    .ConfigureAwait(false);

                return debunkingNews;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting debunking news", request);
                throw;
            }
        }
    }
}
