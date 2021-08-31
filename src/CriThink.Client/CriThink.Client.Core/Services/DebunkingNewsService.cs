using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Exceptions;
using CriThink.Common.Endpoints.DTOs.Admin;
using Microsoft.Extensions.Logging;

namespace CriThink.Client.Core.Services
{
    public class DebunkingNewsService : IDebunkingNewsService
    {
        private readonly IDebunkingNewsApi _debunkingNewsApi;
        private readonly IGeolocationService _geoService;
        private readonly ILogger<DebunkingNewsService> _logger;

        public DebunkingNewsService(IDebunkingNewsApi debunkingNewsApi, IGeolocationService geoService, ILogger<DebunkingNewsService> logger)
        {
            _debunkingNewsApi = debunkingNewsApi ?? throw new ArgumentNullException(nameof(debunkingNewsApi));
            _geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
            _logger = logger;
        }

        public async Task<DebunkingNewsGetAllResponse> GetRecentDebunkingNewsOfCurrentCountryAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var request = new DebunkingNewsGetAllRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageFilters = GetAllDebunkingNewsLanguageFiltersRequest.None,
            };

            var currentArea = await _geoService.GetCurrentCountryCodeAsync().ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(currentArea))
            {
                switch (currentArea)
                {
                    case "it":
                        request.LanguageFilters = GetAllDebunkingNewsLanguageFiltersRequest.Italian;
                        break;
                    case "gb":
                    case "us":
                        request.LanguageFilters = GetAllDebunkingNewsLanguageFiltersRequest.English;
                        break;
                }
            }

            return await GetDebunkingNewsAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<DebunkingNewsGetAllResponse> GetDebunkingNewsAsync(DebunkingNewsGetAllRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            _logger?.LogInformation($"Querying {request.LanguageFilters} debunking news");

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
                _logger?.LogError(ex, "Can't get recent debunking news");
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
                _logger?.LogError(ex, "Can't get debunking news details");
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
