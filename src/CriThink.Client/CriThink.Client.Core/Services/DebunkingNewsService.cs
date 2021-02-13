using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.HttpRepository;
using MvvmCross.Logging;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Services
{
    public class DebunkingNewsService : IDebunkingNewsService
    {
        private readonly IRestRepository _restRepository;
        private readonly IIdentityService _identityService;
        private readonly IGeolocationService _geoService;
        private readonly IMvxLog _log;

        public DebunkingNewsService(IRestRepository restRepository, IIdentityService identityService, IGeolocationService geoService, IMvxLogProvider logProvider)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
            _log = logProvider?.GetLogFor<DebunkingNewsService>();
        }

        public async Task<DebunkingNewsGetAllResponse> GetRecentDebunkingNewsOfCurrentCountryAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var request = new DebunkingNewsGetAllRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageFilters = DebunkingNewsGetAllLanguageFilterRequests.None,
            };

            var token = await _identityService.GetUserTokenAsync().ConfigureAwait(false);
            var currentArea = await _geoService.GetCurrentCountryCodeAsync().ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(currentArea))
            {
                switch (currentArea)
                {
                    case "it":
                        request.LanguageFilters = DebunkingNewsGetAllLanguageFilterRequests.Italian;
                        break;
                    case "gb":
                    case "us":
                        request.LanguageFilters = DebunkingNewsGetAllLanguageFilterRequests.English;
                        break;
                }
            }

            try
            {
                var debunkingNewsCollection = await _restRepository.MakeRequestAsync<DebunkingNewsGetAllResponse>(
                        $"{EndpointConstants.DebunkNewsBase}{EndpointConstants.DebunkingNewsGetAll}?{request.ToQueryString()}",
                        HttpRestVerb.Get,
                        token,
                        cancellationToken)
                    .ConfigureAwait(false);

                return debunkingNewsCollection;
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Can't get recent debunking news", ex);
                return new DebunkingNewsGetAllResponse(null, false);
            }
        }

        public async Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsByIdAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DebunkingNewsGetRequest
            {
                Id = Guid.Parse(id)
            };

            var token = await _identityService.GetUserTokenAsync().ConfigureAwait(false);

            try
            {
                var debunkingNewsDetails = await _restRepository.MakeRequestAsync<DebunkingNewsGetDetailsResponse>(
                        $"{EndpointConstants.DebunkNewsBase}?{request.ToQueryString()}",
                        HttpRestVerb.Get,
                        token,
                        cancellationToken)
                    .ConfigureAwait(false);

                return debunkingNewsDetails;
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Can't get debunking news details", ex);
                return null;
            }
        }

        public async Task OpenDebunkingNewsInBrowser(string link)
        {
            try
            {
                var uri = new Uri(link, UriKind.Absolute);
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error launching the browser with debunking news", ex, link);
            }
        }
    }

    public interface IDebunkingNewsService
    {
        /// <summary>
        /// Retrieve latest debunking news
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Number of debunking news per page</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<DebunkingNewsGetAllResponse> GetRecentDebunkingNewsOfCurrentCountryAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Returns details of the given debunking news id
        /// </summary>
        /// <param name="id">Debunking news id</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsByIdAsync(string id, CancellationToken cancellationToken);

        Task OpenDebunkingNewsInBrowser(string link);
    }
}
