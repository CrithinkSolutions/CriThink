using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerViewModel : BaseBottomViewViewModel, IDisposable
    {
        private const int PageSize = 15;

        private readonly IDebunkingNewsService _debunkingNewsService;
        private readonly IIdentityService _identityService;

        private int _pageIndex = 1;
        private bool _hasMorePages;
        private CancellationTokenSource _cancellationTokenSource;

        public NewsCheckerViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IIdentityService identityService, IDebunkingNewsService debunkingNewsService)
            : base(logProvider, navigationService)
        {
            TabId = "news_checker";

            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));

            Feed = new MvxObservableCollection<DebunkingNewsGetResponse>();
            _hasMorePages = true;
        }

        #region Properties

        public MvxObservableCollection<DebunkingNewsGetResponse> Feed { get; }

        public MvxNotifyTask FetchDebunkingNewsTask { get; private set; }

        private string _newsLinkText;
        public string NewsLinkText
        {
            get => _newsLinkText;
            set => SetProperty(ref _newsLinkText, value);
        }

        private string _welcomeText;
        public string WelcomeText
        {
            get => _welcomeText;
            set => SetProperty(ref _welcomeText, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _todayDate;
        public string TodayDate
        {
            get => _todayDate;
            set => SetProperty(ref _todayDate, value);
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _navigateNewsCheckerCommand;
        public IMvxAsyncCommand NavigateNewsCheckerCommand => _navigateNewsCheckerCommand ??= new MvxAsyncCommand(DoNavigateNewsCheckerCommand);

        private IMvxCommand _fetchDebunkingNewsCommand;
        public IMvxCommand FetchDebunkingNewsCommand => _fetchDebunkingNewsCommand ??= new MvxCommand(DoFetchDebunkingNewsCommand);

        private IMvxCommand<DebunkingNewsGetResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<DebunkingNewsGetResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<DebunkingNewsGetResponse>(DoDebunkingNewsSelectedCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();

            var currentDate = DateTime.Now;

            var currentHour = currentDate.Hour;
            var localizedString = currentHour switch
            {
                var time when time >= 5 && time < 12 => "GoodMorning",
                var time when time >= 12 && time < 18 => "GoodAfternoon",
                var time when time >= 18 && time < 20 => "GoodEvening",
                _ => "GoodNight"
            };

            WelcomeText = LocalizedTextSource.GetText(localizedString);
            TodayDate = currentDate.ToString("D", CultureInfo.CurrentCulture);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

#pragma warning disable 4014
            GetDebunkingNewsAsync();
#pragma warning restore 4014

            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            if (user is null)
                return;

            Username = user.UserName;
        }

        #region Privates

        private async Task GetDebunkingNewsAsync()
        {
            if (!_hasMorePages)
                return;

            IsLoading = true;

            try
            {
                _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var debunkinNewsCollection = await _debunkingNewsService
                    .GetRecentDebunkingNewsAsync(_pageIndex, PageSize, _cancellationTokenSource.Token)
                    .ConfigureAwait(false);

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

        private async Task DoNavigateNewsCheckerCommand(CancellationToken cancellationToken)
        {
            await NavigationService.Navigate<CheckNewsViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);
        }

        private async Task DoCheckNewsCommand(CancellationToken cancellationToken)
        {
            var isValid = UriHelper.IsValidWebSite(NewsLinkText);
            if (!isValid)
                return;

            var uri = new Uri(NewsLinkText);
            await NavigationService.Navigate<NewsCheckerResultViewModel, Uri>(uri, cancellationToken: cancellationToken).ConfigureAwait(true);
        }

        private async Task DoDebunkingNewsSelectedCommand(DebunkingNewsGetResponse selectedResponse, CancellationToken cancellationToken)
        {
            await _debunkingNewsService.OpenDebunkingNewsInBrowser(selectedResponse.NewsLink).ConfigureAwait(false);
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
