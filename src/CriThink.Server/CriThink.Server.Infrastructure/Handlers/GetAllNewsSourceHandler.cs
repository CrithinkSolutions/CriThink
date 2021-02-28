using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class GetAllNewsSourceHandler : IRequestHandler<GetAllNewsSourceQuery, IList<GetAllNewsSourceQueryResponse>>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly ILogger<GetAllNewsSourceHandler> _logger;

        public GetAllNewsSourceHandler(INewsSourceRepository newsSourceRepository, ILogger<GetAllNewsSourceHandler> logger)
        {
            _newsSourceRepository = newsSourceRepository ?? throw new ArgumentNullException(nameof(newsSourceRepository));
            _logger = logger;
        }

        public Task<IList<GetAllNewsSourceQueryResponse>> Handle(GetAllNewsSourceQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var size = request.Size;
                var index = request.Index;
                var filter = request.Filter;

                var redisValues = _newsSourceRepository.GetAllSearchNewsSources();

                IList<GetAllNewsSourceQueryResponse> responses = new List<GetAllNewsSourceQueryResponse>();

                foreach (var (redisKey, redisValue) in ApplyQueryFilter(redisValues, size, index, filter))
                {
                    var newsLink = redisKey.ToString();

                    var isValid = Enum.TryParse(redisValue.ToString(), true, out NewsSourceAuthenticity authenticity);
                    if (!isValid)
                        continue;

                    var response = new GetAllNewsSourceQueryResponse(newsLink, authenticity);
                    responses.Add(response);
                }

                return Task.FromResult(responses);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all news source", request);
                throw;
            }
        }

        private static IEnumerable<Tuple<RedisKey, RedisValue>> ApplyQueryFilter(
            IEnumerable<Tuple<RedisKey, RedisValue>> query,
            int size, int index,
            GetAllNewsSourceFilter filter)
        {
            return query
                .Skip(size * index)
                .Take(size + 1)
                .Where(k =>
                    Enum.TryParse(k.Item2.ToString(), true, out NewsSourceAuthenticity authenticity) &&
                    IsSameCategory(filter, authenticity));
        }

        private static bool IsSameCategory(GetAllNewsSourceFilter filter, NewsSourceAuthenticity authenticity)
        {
            return filter switch
            {
                GetAllNewsSourceFilter.Blacklist => authenticity == NewsSourceAuthenticity.Conspiracist ||
                                                    authenticity == NewsSourceAuthenticity.FakeNews ||
                                                    authenticity == NewsSourceAuthenticity.Suspicious,
                GetAllNewsSourceFilter.Whitelist => authenticity == NewsSourceAuthenticity.Reliable ||
                                                    authenticity == NewsSourceAuthenticity.Satirical ||
                                                    authenticity == NewsSourceAuthenticity.SocialMedia,
                _ => true
            };
        }
    }
}
