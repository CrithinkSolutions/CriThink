using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.Admin;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerViewModel : BaseBottomViewViewModel
    {
        private const int PageSize = 10;

        private readonly IDebunkingNewsService _debunkingNewsService;
        private readonly IIdentityService _identityService;
        private readonly IMvxLog _log;

        private bool _isInitialized;

        public NewsCheckerViewModel(
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService,
            IIdentityService identityService,
            IDebunkingNewsService debunkingNewsService)
            : base(logProvider, navigationService)
        {
            TabId = "news_checker";

            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
            _log = logProvider?.GetLogFor<NewsCheckerViewModel>();

            Feed = new MvxObservableCollection<DebunkingNewsGetResponse>();

            LogoImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        public MvxObservableCollection<DebunkingNewsGetResponse> Feed { get; }

        public List<ITransformation> LogoImageTransformations { get; }

        // TODO: real pic
        public string ProfileImagePath => "res:ic_text_logo";

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


        private IMvxCommand<DebunkingNewsGetResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<DebunkingNewsGetResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<DebunkingNewsGetResponse>(DoDebunkingNewsSelectedCommand);

        private IMvxAsyncCommand _navigateToAllDebunkingNewsCommand;
        public IMvxAsyncCommand NavigateToAllDebunkingNewsCommand => _navigateToAllDebunkingNewsCommand ??=
            new MvxAsyncCommand(DoNavigateToAllDebunkingNewsCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();

            if (_isInitialized)
                return;

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

            if (_isInitialized)
                return;

#pragma warning disable 4014
            GetDebunkingNewsAsync();
#pragma warning restore 4014

            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(true);
            if (user is null)
                return;

            Username = user.UserName;

            _isInitialized = true;
        }

        #region Privates

        private async Task GetDebunkingNewsAsync()
        {
            IsLoading = true;

            try
            {
                var debunkinNewsCollection = await _debunkingNewsService
                    .GetRecentDebunkingNewsOfCurrentCountryAsync(0, PageSize, default)
                    .ConfigureAwait(true);

                if (debunkinNewsCollection.DebunkingNewsCollection != null)
                    Feed.AddRange(debunkinNewsCollection.DebunkingNewsCollection);
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

        private async Task DoDebunkingNewsSelectedCommand(DebunkingNewsGetResponse selectedResponse, CancellationToken cancellationToken)
        {
            await _debunkingNewsService.OpenDebunkingNewsInBrowser(selectedResponse.NewsLink).ConfigureAwait(false);
            _log?.Info("User opens debunking news", selectedResponse.NewsLink);
        }

        private async Task DoNavigateToAllDebunkingNewsCommand(CancellationToken cancellationToken)
        {
            await NavigationService.Navigate<DebunkingNewsListViewModel>(cancellationToken: cancellationToken);
        }

        #endregion
    }
}
