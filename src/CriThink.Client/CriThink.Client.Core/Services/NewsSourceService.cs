using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Entities;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Client.Core.Repositories;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Common;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Common.HttpRepository;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly IRestRepository _restRepository;
        private readonly IIdentityService _identityService;
        private readonly ISQLiteRepository _sqlRepo;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        public NewsSourceService(IRestRepository restRepository, IIdentityService identityService, ISQLiteRepository sqlRepo, IMvxMessenger messenger, IMvxLogProvider logProvider)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _sqlRepo = sqlRepo ?? throw new ArgumentNullException(nameof(sqlRepo));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _log = logProvider?.GetLogFor<NewsSourceService>();
        }

        public async Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var request = new SimpleUriRequest
            {
                Uri = uri.ToString()
            };

            var token = await _identityService.GetUserTokenAsync().ConfigureAwait(false);

            NewsSourceSearchWithDebunkingNewsResponse searchResponse = null;

            try
            {
                searchResponse = await _restRepository.MakeRequestAsync<NewsSourceSearchWithDebunkingNewsResponse>(
                         $"{EndpointConstants.NewsSourceBase}{EndpointConstants.NewsSourceSearch}?{request.ToQueryString()}",
                         HttpRestVerb.Get,
                         token,
                         cancellationToken)
                     .ConfigureAwait(false);

                if (searchResponse.Classification != NewsSourceClassification.Unknown)
                {
                    var recentSearchRequest = new RecentNewsChecksModel
                    {
                        // TODO: Replce with real image
                        NewsImageLink = "res:ic_text_logo",
                        Classification = searchResponse.Classification.ToString(),
                        NewsLink = request.Uri,
                        SearchDateTime = DateTime.Now,
                    };

                    await AddLatestNewsCheckAsync(recentSearchRequest).ConfigureAwait(false);
                }

                return searchResponse
;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error searching a news source", ex, request.Uri);
                return searchResponse;
            }
        }

        public async Task<IList<RecentNewsChecksModel>> GetLatestNewsChecks()
        {
            var entities = await _sqlRepo.GetLatestNewsChecks().ConfigureAwait(false);

            var models = entities.Select(e =>
                new RecentNewsChecksModel
                {
                    Id = e.Id,
                    Classification = e.Classification,
                    NewsLink = e.NewsLink,
                    SearchDateTime = e.SearchDateTime,
                    NewsImageLink = e.NewsImageLink
                }).ToList();

            return models;
        }

        public async Task RegisterForNotificationAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            var currentUser = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            var userEmail = currentUser.UserEmail;

            var request = new NewsSourceNotificationForUnknownDomainRequest
            {
                Uri = uri.ToString(),
                Email = userEmail,
            };

            var token = await _identityService.GetUserTokenAsync().ConfigureAwait(false);

            try
            {
                await _restRepository.MakeRequestAsync(
                    $"{EndpointConstants.NewsSourceBase}{EndpointConstants.NewsSourceRegisterForNotification}",
                    HttpRestVerb.Post,
                    request,
                    token,
                    cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error registering for notification", ex, uri);
            }
        }

        private async Task AddLatestNewsCheckAsync(RecentNewsChecksModel newsCheck)
        {
            if (newsCheck == null)
                throw new ArgumentNullException(nameof(newsCheck));

            var entity = new LatestNewsCheck
            {
                Classification = newsCheck.Classification,
                NewsLink = newsCheck.NewsLink,
                SearchDateTime = newsCheck.SearchDateTime,
                NewsImageLink = newsCheck.NewsImageLink,
            };

            await _sqlRepo.AddLatestNewsCheck(entity).ConfigureAwait(false);

            ClearRecentNewsChecksCache();
        }

        private void ClearRecentNewsChecksCache()
        {
            var message = new ClearRecentNewsSourceCacheMessage(this);
            _messenger.Publish(message);
        }
    }

    public interface INewsSourceService
    {
        Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken);

        Task<IList<RecentNewsChecksModel>> GetLatestNewsChecks();

        Task RegisterForNotificationAsync(Uri uri, CancellationToken cancellationToken);
    }
}
