using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class WebViewNewsViewModel : BaseViewModel<string>
    {
        public MvxObservableCollection<NewsSourceGetQuestionViewModel> Questions { get; private set; }

        private readonly IMvxNavigationService _navigationService;
        private readonly ILogger<NewsCheckerResultViewModel> _logger;
        private readonly INewsSourceService _newsSourceService;
        private CancellationTokenSource _cancellationTokenSource;

        public WebViewNewsViewModel(
            ILogger<NewsCheckerResultViewModel> logger,
            IMvxNavigationService navigationService,
            INewsSourceService newsSourceService
            )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _logger = logger;

            Questions = new MvxObservableCollection<NewsSourceGetQuestionViewModel>();
        }

        public async override Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
           await GetQuestionsAsync().ConfigureAwait(false);
        }

        private async Task GetQuestionsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(45));
            var response = await _newsSourceService.GetQuestionsNewsAsync(_cancellationTokenSource.Token);
            var questions = response.Select(x => new NewsSourceGetQuestionViewModel(x));
            foreach(var question in response)
            {
                Questions.Add(new NewsSourceGetQuestionViewModel(question));
            }
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

        private IMvxCommand _submitNewsQuestionsCommand;
        public IMvxCommand SubmitNewsQuestionsCommand =>
            _submitNewsQuestionsCommand ??= new MvxAsyncCommand(DoSubmitNewsQuestionCommand);

        private async Task DoSubmitNewsQuestionCommand(CancellationToken cancellationToken)
        {
            try
            {
                var questions = new List<NewsSourcePostAnswerRequest>(Questions.Count);
                foreach(var question in Questions)
                {
                    questions.Add(question.CreateNewsSourcePostAnswerRequest());
                }
                var response = await _newsSourceService.PostAnswersToArticleQuestionsAsync(Uri.AbsoluteUri, questions, cancellationToken)
                                .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        #endregion

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            _cancellationTokenSource?.Dispose();
        }
    }
}
