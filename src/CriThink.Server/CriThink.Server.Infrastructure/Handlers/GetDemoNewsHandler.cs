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
    internal class GetDemoNewsHandler : IRequestHandler<GetDemoNewsQuery, DemoNews>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetQuestionHandler> _logger;

        public GetDemoNewsHandler(CriThinkDbContext dbContext, ILogger<GetQuestionHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<DemoNews> Handle(GetDemoNewsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var allQuestions = await _dbContext.DemoNews
                    .GetDemoNewsByIdAsync(request.DemoNewsId, cancellationToken)
                    .ConfigureAwait(false);

                return allQuestions;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting single demo news", request);
                throw;
            }
        }
    }
}
