using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.Search;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class SearchTextResultViewModel : BaseViewModel<string>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly INewsSourceService _newsSourceService;
        private readonly ILogger<SearchTextResultViewModel> _logger;
        private readonly List<BaseNewsSourceSearch> _feed;

        private string _searchText;

        public SearchTextResultViewModel(
            IMvxNavigationService navigationService,
            INewsSourceService newsSourceService,
            ILogger<SearchTextResultViewModel> logger)
        {
            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            _newsSourceService = newsSourceService ??
                throw new ArgumentNullException(nameof(newsSourceService));

            _logger = logger;

            _feed = new List<BaseNewsSourceSearch>();
        }

        #region Properties

        public MvxObservableCollection<BaseNewsSourceSearch> Feed
        {
            get
            {
                if (_feed is null || !_feed.Any())
                    return new MvxObservableCollection<BaseNewsSourceSearch>();

                var feedQuery = _feed.AsEnumerable();

                if (FilterByCommunity)
                {
                    feedQuery = feedQuery.OfType<NewsSourceSearchByTextResponse>();
                }
                else if (FilterByDebunkingNews)
                {
                    feedQuery = feedQuery.OfType<NewsSourceRelatedDebunkingNewsResponse>();
                }

                feedQuery = feedQuery.OrderByDescending(x => x.PublishingDate);

                return new MvxObservableCollection<BaseNewsSourceSearch>(feedQuery);
            }
        }

        private bool _filterByDebunkingNews;
        public bool FilterByDebunkingNews
        {
            get => _filterByDebunkingNews;
            set
            {
                SetProperty(ref _filterByDebunkingNews, value);
                RaisePropertyChanged(nameof(Feed));
            }
        }

        private bool _filterByCommunity;
        public bool FilterByCommunity
        {
            get => _filterByCommunity;
            set
            {
                SetProperty(ref _filterByCommunity, value);
                RaisePropertyChanged(nameof(Feed));
            }
        }

        #endregion

        #region Commands

        private IMvxCommand<NewsSourceRelatedDebunkingNewsResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<NewsSourceRelatedDebunkingNewsResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<BaseNewsSourceSearch>(DoDebunkingNewsSelectedCommand);

        private IMvxCommand _filterByCommunityCommand;
        public IMvxCommand FilterByCommunityCommand =>
            _filterByCommunityCommand ??= new MvxCommand(DoFilterByCommunityCommand);

        private IMvxCommand _filterByDebunkingNewsCommand;
        public IMvxCommand FilterByDebunkingNewsCommand =>
            _filterByDebunkingNewsCommand ??= new MvxCommand(DoFilterByDebunkingNewsCommand);

        #endregion

        public override void Prepare(string searchText)
        {
            if (searchText == null)
            {
                var argumentNullException = new ArgumentNullException(nameof(searchText));
                _logger?.LogCritical(argumentNullException, "The given paramter is null");
                throw argumentNullException;
            }

            _searchText = searchText;
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            await SearchTextAsync().ConfigureAwait(true);
        }

        public async Task NavigateToHomeAsync()
        {
            await _navigationService.Navigate<HomeViewModel>(
                new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    }))
                .ConfigureAwait(true);
        }

        private async Task SearchTextAsync()
        {
            IsLoading = true;

            try
            {
                var results = await _newsSourceService.SearchByTextAsync(_searchText);
                if (results is not null)
                {
                    SetDebunkingNews(results);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DoDebunkingNewsSelectedCommand(
            BaseNewsSourceSearch selectedResponse,
            CancellationToken cancellationToken)
        {
            if (selectedResponse is NewsSourceSearchByTextResponse searchTextResponse)
            {
                _logger?.LogInformation("User opens searched news", searchTextResponse.NewsLink);

                await _navigationService
                    .Navigate<WebViewNewsViewModel, string>(searchTextResponse.NewsLink, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            else if (selectedResponse is NewsSourceRelatedDebunkingNewsResponse dNewsResponse)
            {
                _logger?.LogInformation("User opens debunking news", dNewsResponse.NewsLink);

                var response = new DebunkingNewsGetResponse
                {
                    Title = dNewsResponse.Title,
                    NewsLink = dNewsResponse.NewsLink,
                    Id = dNewsResponse.Id,
                    NewsImageLink = dNewsResponse.NewsImageLink,
                    Publisher = dNewsResponse.Publisher,
                };

                await _navigationService.Navigate<DebunkingNewsDetailsViewModel, DebunkingNewsGetResponse>(
                    response,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
        }

        private void SetDebunkingNews(SearchByTextResponse response)
        {
            if (!response.DebunkingNews.Any())
                return;

            _feed.AddRange(response.DebunkingNews);
            _feed.AddRange(response.NewsSources);

            RaisePropertyChanged(nameof(Feed));
        }

        private void DoFilterByCommunityCommand()
        {
            FilterByDebunkingNews = false;
            FilterByCommunity = !FilterByCommunity;
        }

        private void DoFilterByDebunkingNewsCommand()
        {
            FilterByCommunity = false;
            FilterByDebunkingNews = !FilterByDebunkingNews;
        }
    }
}
