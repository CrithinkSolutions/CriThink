using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class WebviewNewsViewModel : BaseViewModel<string>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ILogger<NewsCheckerResultViewModel> _logger;
        private readonly INewsSourceService _newsSourceService;
        private CancellationTokenSource _cancellationTokenSource;

        public WebviewNewsViewModel(
            ILogger<NewsCheckerResultViewModel> logger,
            IMvxNavigationService navigationService,
            INewsSourceService newsSourceService
            )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _logger = logger;
            _newsSourceService = newsSourceService;
        }

        public async override Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await GetQuestionsAsync().ConfigureAwait(false);
        }

        private async Task GetQuestionsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(45));
            var questions = await _newsSourceService.GetQuestionsNewsAsync("en", _cancellationTokenSource.Token);
            await _navigationService.Navigate<QuestionNewsViewModel, IList<NewsSourceGetQuestionResponse>>(questions);
        }

        public override void Prepare(string parameter)
        {
            if (parameter == null)
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _logger?.LogCritical(argumentNullException, "The given paramter is null");
                throw argumentNullException;
            }
            _uri = new Uri(parameter.Trim());
        }

        #region Properties

        private Uri _uri;
        public Uri Uri => _uri;
        #endregion

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            _cancellationTokenSource?.Dispose();
        }
    }
}
