﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.Admin;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsViewModel : BaseBottomViewViewModel, IDisposable
    {
        private const int PageSize = 5;

        private readonly IDebunkingNewsService _debunkingNewsService;

        private int _pageIndex = 1;
        private bool _hasMorePages;
        private CancellationTokenSource _cancellationTokenSource;

        public DebunkingNewsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IDebunkingNewsService debunkingNewsService)
            : base(logProvider, navigationService)
        {
            TabId = "debunking_news";
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));

            Feed = new MvxObservableCollection<DebunkingNewsGetResponse>();
            _hasMorePages = true;
        }

        #region Properties

        public MvxObservableCollection<DebunkingNewsGetResponse> Feed { get; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            private set => SetProperty(ref _isRefreshing, value);
        }

        public MvxNotifyTask FetchDebunkingNewsTask { get; private set; }

        #endregion

        #region Commands

        private IMvxCommand _fetchDebunkingNewsCommand;
        public IMvxCommand FetchDebunkingNewsCommand => _fetchDebunkingNewsCommand ??= new MvxCommand(DoFetchDebunkingNewsCommand);

        private IMvxCommand _refreshDebunkingNewsCommand;
        public IMvxCommand RefreshDebunkingNewsCommand => _refreshDebunkingNewsCommand ??= new MvxAsyncCommand(DoRefreshDebunkingNewsCommand);

        private IMvxCommand<DebunkingNewsGetResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<DebunkingNewsGetResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<DebunkingNewsGetResponse>(DoDebunkingNewsSelectedCommand);

        #endregion

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await GetDebunkingNewsAsync().ConfigureAwait(false);
        }

        #region Privates

        private async Task GetDebunkingNewsAsync()
        {
            if (!_hasMorePages)
                return;

            IsRefreshing = true;

            try
            {
                _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var debunkinNewsCollection = await _debunkingNewsService
                    .GetRecentDebunkingNewsAsync(_pageIndex, PageSize, _cancellationTokenSource.Token)
                    .ConfigureAwait(false);

                Feed.AddRange(debunkinNewsCollection.DebunkingNewsCollection);
                _hasMorePages = debunkinNewsCollection.HasNextPage;

                _pageIndex++;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task DoDebunkingNewsSelectedCommand(DebunkingNewsGetResponse selectedResponse, CancellationToken cancellationToken)
        {
            await NavigationService.Navigate<DebunkingNewsDetailsViewModel, string>(selectedResponse.Id, cancellationToken: cancellationToken).ConfigureAwait(true);
        }

        private async Task DoRefreshDebunkingNewsCommand(CancellationToken cancellationToken)
        {
            _pageIndex = 1;
            _hasMorePages = true;
            Feed.Clear();
            await GetDebunkingNewsAsync().ConfigureAwait(false);
        }

        private void DoFetchDebunkingNewsCommand()
        {
            FetchDebunkingNewsTask = MvxNotifyTask.Create(GetDebunkingNewsAsync);
            RaisePropertyChanged(() => FetchDebunkingNewsTask);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }
        }

        #endregion
    }
}
