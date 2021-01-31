using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

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

        public async Task<IList<GetAllNewsSourceQueryResponse>> Handle(GetAllNewsSourceQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var size = request.Size;
                var index = request.Index;

                var redisValues = request.Filter switch
                {
                    GetAllNewsSourceFilter.All => await _newsSourceRepository.GetAllSearchNewsSourcesAsync(size, index)
                        .ConfigureAwait(false),
                    GetAllNewsSourceFilter.Whitelist => _newsSourceRepository.GetAllGoodNewsSources(size, index),
                    GetAllNewsSourceFilter.Blacklist => _newsSourceRepository.GetAllBadNewsSources(size, index),
                    _ => throw new NotImplementedException(),
                };

                var responses = new List<GetAllNewsSourceQueryResponse>();

                foreach (var (redisKey, redisValue) in redisValues)
                {
                    var uri = new Uri(redisKey.ToString(), UriKind.Absolute);

                    var isValid = Enum.TryParse(redisValue.ToString(), true, out NewsSourceAuthenticity authenticity);
                    if (!isValid)
                        continue;

                    var response = new GetAllNewsSourceQueryResponse(uri, authenticity);
                    responses.Add(response);
                }

                return responses;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all news source", request);
                throw;
            }
        }
    }
}
