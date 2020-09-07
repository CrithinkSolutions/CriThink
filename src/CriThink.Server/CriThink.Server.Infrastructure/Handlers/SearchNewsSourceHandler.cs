using System;
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
    public class SearchNewsSourceHandler : IRequestHandler<SearchNewsSourceQuery, SearchNewsSourceQueryResponse>
    {
        private readonly INewsSourceRepository _newsSourceRepository;

        public SearchNewsSourceHandler(INewsSourceRepository newsSourceRepository)
        {
            _newsSourceRepository = newsSourceRepository;
        }

        public async Task<SearchNewsSourceQueryResponse> Handle(SearchNewsSourceQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var redisValue = await _newsSourceRepository.SearchNewsSourceAsync(request.Uri).ConfigureAwait(false);

            if (redisValue.IsNullOrEmpty)
                return null;

            var isValid = Enum.TryParse(redisValue.ToString(), true, out NewsSourceAuthencity authenticity);
            return isValid ?
                new SearchNewsSourceQueryResponse(authenticity) :
                null;
        }
    }
}
