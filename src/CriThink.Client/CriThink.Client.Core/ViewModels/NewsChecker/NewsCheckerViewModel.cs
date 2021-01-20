using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerViewModel : BaseBottomViewViewModel
    {
        private readonly IIdentityService _identityService;

        public NewsCheckerViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IIdentityService identityService)
            : base(logProvider, navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            TabId = "news_checker";
        }

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

        private IMvxAsyncCommand _navigateNewsCheckerCommand;
        public IMvxAsyncCommand NavigateNewsCheckerCommand => _navigateNewsCheckerCommand ??= new MvxAsyncCommand(DoNavigateNewsCheckerCommand);

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
            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            if (user is null)
                return;

            Username = user.UserName;
        }

        private Task DoNavigateNewsCheckerCommand(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return NavigationService.Navigate<NewsCheckerSearchView>(cancellationToken: cancellationToken);
        }

        private async Task DoCheckNewsCommand(CancellationToken cancellationToken)
        {
            var isValid = UriHelper.IsValidWebSite(NewsLinkText);
            if (!isValid)
                return;

            var uri = new Uri(NewsLinkText);
            await NavigationService.Navigate<NewsCheckerResultViewModel, Uri>(uri, cancellationToken: cancellationToken).ConfigureAwait(true);
        }
    }
}
