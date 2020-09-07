using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class GetAllNewsSourceHandler : IRequestHandler<GetAllNewsSourceQuery, IEnumerable<GetAllNewsSourceQueryResponse>>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        public GetAllNewsSourceHandler(INewsSourceRepository newsSourceRepository)
        {
            _newsSourceRepository = newsSourceRepository ?? throw new ArgumentNullException(nameof(newsSourceRepository));
        }

        public async Task<IEnumerable<GetAllNewsSourceQueryResponse>> Handle(GetAllNewsSourceQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var redisValues = request.Filter switch
            {
                GetAllNewsSourceFilter.All => await _newsSourceRepository.GetAllSearchNewsSourcesAsync().ConfigureAwait(false),
                GetAllNewsSourceFilter.Whitelist => _newsSourceRepository.GetAllGoodNewsSources(),
                GetAllNewsSourceFilter.Blacklist => _newsSourceRepository.GetAllBadNewsSources(),
                _ => throw new NotImplementedException(),
            };

            var responses = new List<GetAllNewsSourceQueryResponse>();

            foreach (var (redisKey, redisValue) in redisValues)
            {
                var uri = new Uri(redisKey.ToString(), UriKind.Absolute);

                var isValid = Enum.TryParse(redisValue.ToString(), true, out NewsSourceAuthencity authenticity);
                if (!isValid)
                    continue;

                var response = new GetAllNewsSourceQueryResponse(uri, authenticity);
                responses.Add(response);
            }

            return responses;
        }
    }
}
