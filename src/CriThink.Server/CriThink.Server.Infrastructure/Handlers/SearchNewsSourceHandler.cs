using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class SearchNewsSourceHandler : IRequestHandler<SearchNewsSourceQuery, SearchNewsSourceQueryResponse>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<SearchNewsSourceHandler> _logger;

        public SearchNewsSourceHandler(INewsSourceRepository newsSourceRepository, CriThinkDbContext dbContext, ILogger<SearchNewsSourceHandler> logger)
        {
            _logger = logger;
            _newsSourceRepository = newsSourceRepository;
            _dbContext = dbContext;
        }

        public async Task<SearchNewsSourceQueryResponse> Handle(SearchNewsSourceQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var redisValue = await _newsSourceRepository.SearchNewsSourceAsync(request.NewsLink).ConfigureAwait(false);

                if (redisValue.IsNullOrEmpty)
                    return null;

                var isValid = Enum.TryParse(redisValue.ToString(), true, out NewsSourceAuthenticity authenticity);
                if (!isValid)
                    return null;

                var description = await _dbContext.NewsSourceCategories
                    .GetCategoryDescriptionByAuthenticityAsync(NewsSourceCategoryProjection.GetDescription, authenticity, cancellationToken)
                    .ConfigureAwait(false);

                return new SearchNewsSourceQueryResponse(authenticity, description);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error searching news source", request);
                throw;
            }
        }
    }
}
