﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Entities;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Client.Core.Repositories;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Common;
using CriThink.Common.Endpoints.DTOs.NewsSource;
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

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var request = new SimpleUriRequest
            {
                Uri = uri.ToString()
            };

            var token = await _identityService.GetUserTokenAsync().ConfigureAwait(false);

            try
            {
                var loginResponse = await _restRepository.MakeRequestAsync<NewsSourceSearchResponse>(
                        $"{EndpointConstants.NewsSourceBase}?{request.ToQueryString()}",
                        HttpRestVerb.Get,
                        token,
                        cancellationToken)
                    .ConfigureAwait(false);

                return loginResponse;
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error searching a news source", ex, request.Uri);
                return null;
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

        public async Task AddLatestNewsCheck(RecentNewsChecksModel newsCheck)
        {
            if (newsCheck == null)
                throw new ArgumentNullException(nameof(newsCheck));

            // TODO: Replce with real data
            var entity = new LatestNewsCheck
            {
                Classification = newsCheck.Classification,
                NewsLink = newsCheck.NewsLink,
                SearchDateTime = newsCheck.SearchDateTime,
                NewsImageLink = "https://www.open.online/wp-content/uploads/2021/01/GOLF_20210127183930444_1e59b1e069ede758cacf0e791794eb13-1152x768.jpg"
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
        Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken);

        Task<IList<RecentNewsChecksModel>> GetLatestNewsChecks();

        Task AddLatestNewsCheck(RecentNewsChecksModel newsCheck);
    }
}
