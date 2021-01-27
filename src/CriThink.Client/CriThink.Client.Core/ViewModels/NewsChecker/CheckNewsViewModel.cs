using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Client.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Navigation;

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class CheckNewsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly INewsSourceService _newsSourceService;

        public CheckNewsViewModel(IMvxNavigationService navigationService, INewsSourceService newsSourceService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));

            RecentNewsChecksCollection = new ObservableCollection<RecentNewsChecksModel>();
        }

        #region Properties

        public ObservableCollection<RecentNewsChecksModel> RecentNewsChecksCollection { get; }

        private string _newsUri;

        public string NewsUri
        {
            get => _newsUri;
            set => SetProperty(ref _newsUri, value);
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _submitUriCommand;
        public IMvxAsyncCommand SubmitUriCommand => _submitUriCommand ??= new MvxAsyncCommand(DoSubmitUriCommand);

        #endregion

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            var modelCollection = await _newsSourceService.GetLatestNewsChecks().ConfigureAwait(false);
            foreach (var model in modelCollection)
                RecentNewsChecksCollection.Add(model);
        }

        private async Task DoSubmitUriCommand(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(NewsUri))
                return;

            // TODO: do in the next view after results with real data
            await _newsSourceService.AddLatestNewsCheck(
                new RecentNewsChecksModel
                {
                    NewsLink = NewsUri,
                    Classification = "satirical",
                    SearchDateTime = DateTime.Now
                }).ConfigureAwait(false);
        }
    }
}
