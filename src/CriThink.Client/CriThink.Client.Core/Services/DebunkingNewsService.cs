using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Exceptions;
using CriThink.Common.Endpoints.DTOs.Admin;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Services
{
    public class DebunkingNewsService : IDebunkingNewsService
    {
        private readonly IDebunkingNewsApi _debunkingNewsApi;
        private readonly IGeolocationService _geoService;
        private readonly IMvxLog _log;

        public DebunkingNewsService(IDebunkingNewsApi debunkingNewsApi, IGeolocationService geoService, IMvxLogProvider logProvider)
        {
            _debunkingNewsApi = debunkingNewsApi ?? throw new ArgumentNullException(nameof(debunkingNewsApi));
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

            return await GetDebunkingNewsAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<DebunkingNewsGetAllResponse> GetDebunkingNewsAsync(DebunkingNewsGetAllRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            _log?.Info($"Querying {request.LanguageFilters} debunking news");

            try
            {
                DebunkingNewsGetAllResponse debunkingNewsCollection = await _debunkingNewsApi
                    .GetAllDebunkingNewsAsync(request, cancellationToken)
                    .ConfigureAwait(false);

                return debunkingNewsCollection;
            }
            catch (TokensExpiredException)
            {
                throw;
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

            try
            {
                DebunkingNewsGetDetailsResponse debunkingNewsDetails = await _debunkingNewsApi.GetDebunkingNewsAsync(request, cancellationToken)
                    .ConfigureAwait(false);

                return debunkingNewsDetails;
            }
            catch (TokensExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Can't get debunking news details", ex);
                return null;
            }
        }
    }

    public interface IDebunkingNewsService
    {
        /// <summary>
        /// Retrieve latest debunking news of the current country
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Number of debunking news per page</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<DebunkingNewsGetAllResponse> GetRecentDebunkingNewsOfCurrentCountryAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve all the debunking news
        /// </summary>
        /// <param name="request">Pagination settings and filters</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<DebunkingNewsGetAllResponse> GetDebunkingNewsAsync(DebunkingNewsGetAllRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Returns details of the given debunking news id
        /// </summary>
        /// <param name="id">Debunking news id</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsByIdAsync(string id, CancellationToken cancellationToken);
    }
}
