using System;
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
        private const int PageSize = 20;

        private readonly IDebunkingNewsService _debunkingNewsService;

        private int _pageIndex = 1;
        private CancellationTokenSource _cancellationTokenSource;

        public DebunkingNewsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IDebunkingNewsService debunkingNewsService)
            : base(logProvider, navigationService)
        {
            TabId = "debunking_news";
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));

            Feed = new MvxObservableCollection<DebunkingNewsGetAllResponse>();
        }

        #region Properties

        public MvxObservableCollection<DebunkingNewsGetAllResponse> Feed { get; }

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

        private IMvxCommand<DebunkingNewsGetAllResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<DebunkingNewsGetAllResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<DebunkingNewsGetAllResponse>(DoDebunkingNewsSelectedCommand);

        #endregion

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await GetDebunkingNewsAsync().ConfigureAwait(false);
        }

        #region Privates

        private async Task GetDebunkingNewsAsync()
        {
            if (_pageIndex > 5) // TODO: https://github.com/CrithinkSolutions/CriThink/issues/277
                return;

            IsRefreshing = true;

            try
            {
                _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var debunkinNewsCollection = await _debunkingNewsService
                    .GetRecentDebunkingNewsAsync(_pageIndex, PageSize, _cancellationTokenSource.Token)
                    .ConfigureAwait(false);

                Feed.AddRange(debunkinNewsCollection);

                _pageIndex++;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task DoDebunkingNewsSelectedCommand(DebunkingNewsGetAllResponse selectedResponse, CancellationToken cancellationToken)
        {
            await NavigationService.Navigate<DebunkingNewsDetailsViewModel, string>(selectedResponse.Id, cancellationToken: cancellationToken).ConfigureAwait(true);
        }

        private async Task DoRefreshDebunkingNewsCommand(CancellationToken cancellationToken)
        {
            _pageIndex = 1;
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
