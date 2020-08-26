using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;
using MediatR;

namespace CriThink.Server.Infrastructure.Handlers
{
    public class GetAllDemoNewsHandler : IRequestHandler<GetAllDemoNewsQuery, List<GetAllDemoNewsQueryResponse>>
    {
        private readonly CriThinkDbContext _dbContext;

        public GetAllDemoNewsHandler(CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<GetAllDemoNewsQueryResponse>> Handle(GetAllDemoNewsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var allDemoNews = await _dbContext.DemoNews.GetAllDemoNewsAsync(DemoNewsProjection.GetAll, cancellationToken).ConfigureAwait(false);
            return allDemoNews;
        }
    }
}
