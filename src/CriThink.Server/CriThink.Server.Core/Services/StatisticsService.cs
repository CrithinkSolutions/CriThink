using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Statistics;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(IMediator mediator, ILogger<StatisticsService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task<UsersCountingResponse> GetUsersCountingAsync()
        {
            _logger?.LogInformation("Requested users counting");

            var query = new GetStatisticsUserCountingQuery();
            var usersCount = await _mediator.Send(query);

            return new UsersCountingResponse
            {
                Counting = usersCount.UsersCounting,
            };
        }

        public async Task<SearchesCountingResponse> GetTotalSearchesAsync()
        {
            _logger?.LogInformation("Requested total searches counting");

            var query = new GetStatisticsSearchesCountingQuery();
            var searchesCount = await _mediator.Send(query);

            return new SearchesCountingResponse
            {
                TotalSearches = searchesCount.SearchesCounting,
            };
        }
    }
}
