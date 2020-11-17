using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Common;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.HttpRepository;

namespace CriThink.Client.Core.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly IRestRepository _restRepository;

        public NewsSourceService(IRestRepository restRepository)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var request = new SimpleUriRequest
            {
                Uri = uri.ToString()
            };

            var loginResponse = await _restRepository.MakeRequestAsync<NewsSourceSearchResponse>(
                    $"{EndpointConstants.NewsSourceBase}?{request.ToQueryString()}",
                    HttpRestVerb.Get,
                    cancellationToken)
                .ConfigureAwait(false);

            return loginResponse;
        }
    }

    public interface INewsSourceService
    {
        Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken);
    }
}
