using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class GetAllDemoNewsHandler : IRequestHandler<GetAllDemoNewsQuery, List<DemoNews>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllDemoNewsHandler> _logger;

        public GetAllDemoNewsHandler(CriThinkDbContext dbContext, ILogger<GetAllDemoNewsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<List<DemoNews>> Handle(GetAllDemoNewsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var allDemoNews = await _dbContext.DemoNews
                    .GetAllDemoNewsAsync(DemoNewsProjection.GetAll, cancellationToken)
                    .ConfigureAwait(false);

                return allDemoNews;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all demo news");
                throw;
            }
        }
    }
}
