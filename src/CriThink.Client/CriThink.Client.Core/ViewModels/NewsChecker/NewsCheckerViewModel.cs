using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerViewModel : BaseBottomViewViewModel
    {
        public NewsCheckerViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            TabId = "news_checker";
        }

        private string _news;
        public string News
        {
            get => _news;
            set => SetProperty(ref _news, value);
        }

        private IMvxAsyncCommand _checkNewsCommand;
        public IMvxAsyncCommand CheckNewsCommand => _checkNewsCommand ??= new MvxAsyncCommand(DoCheckNewsCommand);

        private async Task DoCheckNewsCommand(CancellationToken cancellationToken)
        {
            var isValid = UriHelper.IsValidWebSite(News);
            if (!isValid)
                return;

            var uri = new Uri(News);
            await NavigationService.Navigate<NewsCheckerResultViewModel, Uri>(uri, cancellationToken: cancellationToken).ConfigureAwait(true);
        }
    }
}
