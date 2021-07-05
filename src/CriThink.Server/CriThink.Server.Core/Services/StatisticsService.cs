using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Statistics;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(IMediator mediator, IHttpContextAccessor httpContext, ILogger<StatisticsService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _logger = logger;
        }

        public async Task<PlatformDataUsageResponse> GetPlatformDataUsageAsync()
        {
            _logger?.LogInformation("Requested platform usage data");

            var userQuery = new GetStatisticsUserCountingQuery();
            var usersCount = await _mediator.Send(userQuery);

            var searchesQuery = new GetStatisticsSearchesCountingQuery();
            var searchesCount = await _mediator.Send(searchesQuery);

            return new PlatformDataUsageResponse
            {
                Counting = usersCount.UsersCounting,
                TotalSearches = searchesCount.SearchesCounting,
            };
        }

        public async Task<SearchesCountingResponse> GetUserTotalSearchesAsync()
        {
            _logger?.LogInformation("Requested user total searches counting");

            var userId = _httpContext.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                throw new InvalidOperationException("No user logged");

            var query = new GetStatisticsSearchesCountingQuery(Guid.Parse(userId));
            var searchesCount = await _mediator.Send(query);

            return new SearchesCountingResponse
            {
                TotalSearches = searchesCount.SearchesCounting,
            };
        }
    }
}
