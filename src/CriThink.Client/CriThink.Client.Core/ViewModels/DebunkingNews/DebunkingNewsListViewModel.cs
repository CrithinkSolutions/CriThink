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
    public class DebunkingNewsListViewModel : BaseViewModel
    {
        private const int PageSize = 20;

        private readonly IDebunkingNewsService _debunkingNewsService;
        private readonly IMvxLog _log;

        private bool _isInitialized;
        private int _pageIndex;
        private bool _hasMorePages;
        private CancellationTokenSource _cancellationTokenSource;

        public DebunkingNewsListViewModel(
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService,
            IDebunkingNewsService debunkingNewsService
            )
        {
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
            _log = logProvider?.GetLogFor<DebunkingNewsListViewModel>();

            Feed = new MvxObservableCollection<DebunkingNewsGetResponse>();
            _hasMorePages = true;
        }

        #region Properties

        public MvxObservableCollection<DebunkingNewsGetResponse> Feed { get; }

        public MvxNotifyTask FetchDebunkingNewsTask { get; private set; }

        #endregion

        #region Commands

        private IMvxCommand _fetchDebunkingNewsCommand;
        public IMvxCommand FetchDebunkingNewsCommand => _fetchDebunkingNewsCommand ??= new MvxCommand(DoFetchDebunkingNewsCommand);


        private IMvxCommand<DebunkingNewsGetResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<DebunkingNewsGetResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<DebunkingNewsGetResponse>(DoDebunkingNewsSelectedCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();

            if (_isInitialized)
                return;

            _log?.Info("User navigates to all debunking news");
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            if (_isInitialized)
                return;

            await GetDebunkingNewsAsync().ConfigureAwait(false);

            _isInitialized = true;
        }

        private async Task GetDebunkingNewsAsync()
        {
            if (!_hasMorePages)
                return;

            IsLoading = true;

            try
            {
                _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                var request = new DebunkingNewsGetAllRequest
                {
                    PageSize = PageSize,
                    PageIndex = _pageIndex,
                    LanguageFilters = DebunkingNewsGetAllLanguageFilterRequests.None,
                };

                var debunkinNewsCollection = await _debunkingNewsService
                    .GetDebunkingNewsAsync(request, _cancellationTokenSource.Token)
                    .ConfigureAwait(false);

                await Task.Delay(3000);

                if (debunkinNewsCollection.DebunkingNewsCollection != null)
                    Feed.AddRange(debunkinNewsCollection.DebunkingNewsCollection);

                _hasMorePages = debunkinNewsCollection.HasNextPage;

                _pageIndex++;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DoDebunkingNewsSelectedCommand(DebunkingNewsGetResponse selectedResponse, CancellationToken cancellationToken)
        {
            await _debunkingNewsService.OpenDebunkingNewsInBrowser(selectedResponse.NewsLink).ConfigureAwait(false);
            _log?.Info("User opens debunking news", selectedResponse.NewsLink);
        }

        private void DoFetchDebunkingNewsCommand()
        {
            FetchDebunkingNewsTask = MvxNotifyTask.Create(GetDebunkingNewsAsync);
            RaisePropertyChanged(() => FetchDebunkingNewsTask);
        }
    }
}