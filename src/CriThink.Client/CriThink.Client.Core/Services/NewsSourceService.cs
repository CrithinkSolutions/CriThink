using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Exceptions;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Entities;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Client.Core.Repositories;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly INewsSourceApi _newsSourceApi;
        private readonly IUserProfileService _userProfileService;
        private readonly ISQLiteRepository _sqlRepo;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        public NewsSourceService(
            INewsSourceApi newsSourceApi,
            IUserProfileService userProfileService,
            ISQLiteRepository sqlRepo,
            IMvxMessenger messenger, IMvxLogProvider logProvider)
        {
            _newsSourceApi = newsSourceApi ?? throw new ArgumentNullException(nameof(newsSourceApi));
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
            _sqlRepo = sqlRepo ?? throw new ArgumentNullException(nameof(sqlRepo));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _log = logProvider?.GetLogFor<NewsSourceService>();
        }

        public async Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceAsync(string newsLink, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            var request = new NewsSourceSearchRequest
            {
                NewsLink = newsLink
            };

            NewsSourceSearchWithDebunkingNewsResponse searchResponse = null;

            try
            {
                searchResponse = await _newsSourceApi.SearchNewsSourceAsync(request, cancellationToken)
                    .ConfigureAwait(false);

                if (searchResponse.Classification != NewsSourceClassification.Unknown)
                {
                    var recentSearchRequest = new RecentNewsChecksModel
                    {
                        // TODO: Replce with real image
                        NewsImageLink = "res:ic_text_logo",
                        Classification = searchResponse.Classification.ToString(),
                        NewsLink = newsLink,
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
                _log?.ErrorException("Error searching a news source", ex, newsLink);
                return searchResponse;
            }
        }

        public async Task<IList<RecentNewsChecksModel>> GetLatestNewsChecksAsync()
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

        public async Task RegisterForNotificationAsync(string newsLink, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            var currentUser = await _userProfileService.GetUserProfileAsync(cancellationToken).ConfigureAwait(false);
            var userEmail = currentUser.Email;

            var request = new NewsSourceNotificationForUnknownDomainRequest
            {
                Uri = newsLink,
                Email = userEmail,
            };

            try
            {
                await _newsSourceApi.RegisterForNotificationAsync(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TokensExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error registering for notification", ex, newsLink);
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
        Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceAsync(string newsLink, CancellationToken cancellationToken);

        Task<IList<RecentNewsChecksModel>> GetLatestNewsChecksAsync();

        Task RegisterForNotificationAsync(string newsLink, CancellationToken cancellationToken);
    }
}
