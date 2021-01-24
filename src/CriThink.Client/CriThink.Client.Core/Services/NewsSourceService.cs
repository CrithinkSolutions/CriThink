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
        private readonly IIdentityService _identityService;

        public NewsSourceService(IRestRepository restRepository, IIdentityService identityService)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var request = new SimpleUriRequest
            {
                Uri = uri.ToString()
            };

            var token = await _identityService.GetUserTokenAsync().ConfigureAwait(false);

            var loginResponse = await _restRepository.MakeRequestAsync<NewsSourceSearchResponse>(
                    $"{EndpointConstants.NewsSourceBase}?{request.ToQueryString()}",
                    HttpRestVerb.Get,
                    token,
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
