using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Exceptions;
using CriThink.Client.Core.Messenger;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.Notification;
using CriThink.Common.Endpoints.DTOs.Search;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Logging;
using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly INewsSourceApi _newsSourceApi;
        private readonly INotificationApi _notificationApi;
        private readonly ISearchApi _searchApi;
        private readonly IMvxMessenger _messenger;
        private readonly IGeolocationService _geoService;
        private readonly ILogger<NewsSourceService> _logger;

        public NewsSourceService(
            INewsSourceApi newsSourceApi,
            INotificationApi notificationApi,
            ISearchApi searchApi,
            IGeolocationService geoService,
            IMvxMessenger messenger,
            ILogger<NewsSourceService> logger)
        {
            _newsSourceApi = newsSourceApi ??
                throw new ArgumentNullException(nameof(newsSourceApi));

            _notificationApi = notificationApi ??
                throw new ArgumentNullException(nameof(notificationApi));
            
            _searchApi = searchApi ??
                throw new ArgumentNullException(nameof(searchApi));

            _geoService = geoService ??
                throw new ArgumentNullException(nameof(geoService));

            _messenger = messenger ??
                throw new ArgumentNullException(nameof(messenger));

            _logger = logger;
        }

        public async Task<IList<NewsSourceGetQuestionResponse>> GetQuestionsNewsAsync(
            CancellationToken cancellationToken = default)
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

        public async Task<NewsSourcePostAnswersResponse> PostAnswersToArticleQuestionsAsync(
            string newsLink,
            IList<NewsSourcePostAnswerRequest> questions,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            var currentArea = await _geoService.GetCurrentCountryCodeAsync().ConfigureAwait(false);

            var request = new NewsSourcePostAllAnswersRequest
            {
                NewsLink = newsLink,
                Questions = questions
            };

            try
            {
                var response = await _newsSourceApi.PostAnswersToArticleQuestionsAsync(
                    currentArea.Coalesce(GeoConstant.DEFAULT_LANGUAGE),
                    request,
                    cancellationToken)
                    .ConfigureAwait(false);

                var message = new ClearRecentNewsSourceCacheMessage(this);
                _messenger.Publish(message);

                return response;
            }
            catch (TokensExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, newsLink);
                throw;
            }
        }

        public async Task RegisterForNotificationAsync(
            string newsLink,
            CancellationToken cancellationToken = default)
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

        public async Task UnregisterForNotificationAsync(string newsLink, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            var request = new NewsSourceCancelNotificationForUnknownDomainRequest
            {
                NewsSource = newsLink,
            };

            try
            {
                await _notificationApi.CancelNotificationForUnknownSourceAsync(request, cancellationToken)
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

        public async Task<SearchByTextResponse> SearchByTextAsync(
            string searchText,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                throw new ArgumentNullException(nameof(searchText));

            try
            {
                var result = await _searchApi.SearchAsync(searchText, cancellationToken);
                return result;
            }
            catch (TokensExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error searching for text", searchText);
                return null;
            }
        }
    }

    public interface INewsSourceService
    {
        Task<IList<NewsSourceGetQuestionResponse>> GetQuestionsNewsAsync(CancellationToken cancellationToken = default);

        Task<NewsSourcePostAnswersResponse> PostAnswersToArticleQuestionsAsync(string newsLink, IList<NewsSourcePostAnswerRequest> questions, CancellationToken cancellationToken);

        Task RegisterForNotificationAsync(string newsLink, CancellationToken cancellationToken = default);

        Task UnregisterForNotificationAsync(string newsLink, CancellationToken cancellationToken = default);

        Task<SearchByTextResponse> SearchByTextAsync(
            string searchText,
            CancellationToken cancellationToken = default);
    }
}
