using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Exceptions;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Common.Endpoints.DTOs.Admin;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsListViewModel : BaseViewModel
    {
        private const int PageSize = 20;

        private readonly IMvxNavigationService _navigationService;
        private readonly IDebunkingNewsService _debunkingNewsService;
        private readonly ILogger<DebunkingNewsListViewModel> _logger;

        private bool _isInitialized;
        private int _pageIndex;
        private bool _hasMorePages;
        private CancellationTokenSource _cancellationTokenSource;

        public DebunkingNewsListViewModel(
            ILogger<DebunkingNewsListViewModel> logger,
            IMvxNavigationService navigationService,
            IDebunkingNewsService debunkingNewsService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
            _logger = logger;

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

            _logger?.LogInformation("User navigates to all debunking news");
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
                    LanguageFilters = GetAllDebunkingNewsLanguageFiltersRequest.None,
                };

                var debunkinNewsCollection = await _debunkingNewsService
                    .GetDebunkingNewsAsync(request, _cancellationTokenSource.Token)
                    .ConfigureAwait(false);

                if (debunkinNewsCollection.DebunkingNewsCollection != null)
                    Feed.AddRange(debunkinNewsCollection.DebunkingNewsCollection);

                _hasMorePages = debunkinNewsCollection.HasNextPage;

                _pageIndex++;
            }
            catch (TokensExpiredException)
            {
                await _navigationService.Navigate<SignUpViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    })).ConfigureAwait(true);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DoDebunkingNewsSelectedCommand(DebunkingNewsGetResponse selectedResponse, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("User opens debunking news", selectedResponse.NewsLink);

            await _navigationService.Navigate<DebunkingNewsDetailsViewModel, DebunkingNewsGetResponse>(selectedResponse, cancellationToken: cancellationToken)
                .ConfigureAwait(true);
        }

        private void DoFetchDebunkingNewsCommand()
        {
            FetchDebunkingNewsTask = MvxNotifyTask.Create(GetDebunkingNewsAsync);
            RaisePropertyChanged(() => FetchDebunkingNewsTask);
        }
    }
}
