using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Exceptions;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Entities;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Client.Core.Repositories;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.Notification;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly INewsSourceApi _newsSourceApi;
        private readonly INotificationApi _notificationApi;
        private readonly IUserProfileService _userProfileService;
        private readonly ISQLiteRepository _sqlRepo;
        private readonly IMvxMessenger _messenger;
        private readonly IGeolocationService _geoService;
        private readonly ILogger<NewsSourceService> _logger;

        public NewsSourceService(
            INewsSourceApi newsSourceApi,
            INotificationApi notificationApi,
            IUserProfileService userProfileService,
            ISQLiteRepository sqlRepo,
            IGeolocationService geoService,
            IMvxMessenger messenger, ILogger<NewsSourceService> logger)
        {
            _newsSourceApi = newsSourceApi ?? throw new ArgumentNullException(nameof(newsSourceApi));
            _notificationApi = notificationApi ?? throw new ArgumentNullException(nameof(notificationApi)); ;
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
            _sqlRepo = sqlRepo ?? throw new ArgumentNullException(nameof(sqlRepo));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
            _logger = logger;
        }

        public async Task<IList<RecentNewsChecksModel>> GetLatestNewsChecksAsync(IMvxAsyncCommand<RecentNewsChecksModel> deleteHistoryRecentNewsItemCommand)
        {
            var entities = await _sqlRepo.GetLatestNewsChecks().ConfigureAwait(false);

            var models = entities.Select(e =>
                new RecentNewsChecksModel(deleteHistoryRecentNewsItemCommand)
                {
                    Id = e.Id,
                    Classification = e.Classification,
                    NewsLink = e.NewsLink,
                    SearchDateTime = e.SearchDateTime,
                    NewsImageLink = e.NewsImageLink
                }).ToList();

            return models;
        }

        public async Task<IList<NewsSourceGetQuestionResponse>> GetQuestionsNewsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var currentArea = await _geoService.GetCurrentCountryCodeAsync().ConfigureAwait(false);

                var response = await _newsSourceApi.GetNewsSourceQuestionsAsync(
                    currentArea.Coalesce(GeoConstant.DEFAULT_LANGUAGE),
                    cancellationToken)
                    .ConfigureAwait(false);

                return response.Questions;
            }
            catch (TokensExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieve questions for news");
                throw;
            }
        }

        public async Task<NewsSourcePostAnswersResponse> PostAnswersToArticleQuestionsAsync(string newsLink, IList<NewsSourcePostAnswerRequest> questions, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            if (questions == null)
                throw new ArgumentNullException(nameof(questions));

            var request = new NewsSourcePostAllAnswersRequest
            {
                NewsLink = newsLink,
                Questions = questions
            };
            try
            {
                var response = await _newsSourceApi.PostAnswersToArticleQuestionsAsync(request, cancellationToken)
                            .ConfigureAwait(false);
                return response;

            }
            catch (TokensExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error registering for notification", newsLink);
                throw;
            }
        }

        public async Task RegisterForNotificationAsync(string newsLink, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            var request = new NewsSourceNotificationForUnknownDomainRequest
            {
                NewsSource = newsLink,
            };

            try
            {
                await _notificationApi.RequestNotificationForUnknownSourceAsync(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TokensExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error registering for notification", newsLink);
            }
        }

        // TODO: use again
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
        Task<IList<RecentNewsChecksModel>> GetLatestNewsChecksAsync(IMvxAsyncCommand<RecentNewsChecksModel> deleteHistoryRecentNewsItemCommand);

        Task<IList<NewsSourceGetQuestionResponse>> GetQuestionsNewsAsync(CancellationToken cancellationToken = default);

        Task<NewsSourcePostAnswersResponse> PostAnswersToArticleQuestionsAsync(string newsLink, IList<NewsSourcePostAnswerRequest> questions, CancellationToken cancellationToken);

        Task RegisterForNotificationAsync(string newsLink, CancellationToken cancellationToken);
    }
}
