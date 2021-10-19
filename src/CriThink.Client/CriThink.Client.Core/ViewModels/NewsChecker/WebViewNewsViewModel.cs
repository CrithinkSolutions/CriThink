using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Models.NewsChecker;
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
        private readonly ILogger<NewsCheckerResultViewModel> _logger;
        private readonly INewsSourceService _newsSourceService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxNavigationService _navigationService;

        public WebViewNewsViewModel(
            ILogger<NewsCheckerResultViewModel> logger,
            INewsSourceService newsSourceService,
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs)
        {
            _newsSourceService = newsSourceService ??
                throw new ArgumentNullException(nameof(newsSourceService));
            _userDialogs = userDialogs ??
                throw new ArgumentNullException(nameof(userDialogs));
            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));
            _logger = logger;

            Questions = new MvxObservableCollection<NewsSourceGetQuestionViewModel>();
        }

        #region Properties

        private Uri _uri;
        public Uri Uri => _uri;

        private IMvxCommand _submitNewsQuestionsCommand;
        public IMvxCommand SubmitNewsQuestionsCommand =>
            _submitNewsQuestionsCommand ??= new MvxAsyncCommand(DoSubmitNewsQuestionCommand);

        public MvxObservableCollection<NewsSourceGetQuestionViewModel> Questions { get; private set; }

        #endregion

        public async override Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await GetQuestionsAsync().ConfigureAwait(false);
        }

        private async Task GetQuestionsAsync()
        {
            var response = await _newsSourceService.GetQuestionsNewsAsync();
            var questions = response.Select(nsgr => new NewsSourceGetQuestionViewModel(nsgr));

            foreach (var question in response)
            {
                Questions.Add(new NewsSourceGetQuestionViewModel(question));
            }
        }

        public override void Prepare(string parameter)
        {
            if (parameter is null)
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _logger?.LogCritical(argumentNullException, "The given paramter is null");
                throw argumentNullException;
            }

            _uri = new Uri(parameter.Trim());
        }

        private async Task DoSubmitNewsQuestionCommand(CancellationToken cancellationToken)
        {
            try
            {

                var questions = new List<NewsSourcePostAnswerRequest>(Questions.Count);

                foreach (var question in Questions)
                {
                    if (question.SelectedVote == 0)
                    {
                        var title = LocalizedTextSource.GetText("MissingVoteTitle");
                        var message = LocalizedTextSource.GetText("MissingVoteMessage");
                        var ok = LocalizedTextSource.GetText("MissingVoteConfirm");
                        await ShowFormatMessageAsync(message, title, ok, cancellationToken);

                        return;
                    }

                    questions.Add(question.CreateNewsSourcePostAnswerRequest());
                }

                using var progress = _userDialogs.Loading(maskType: MaskType.Clear, title: string.Empty);

                var response = await _newsSourceService.PostAnswersToArticleQuestionsAsync(
                   Uri.AbsoluteUri,
                    questions,
                    cancellationToken)
                    .ConfigureAwait(false);

                var newsCheckerResultModel = NewsCheckerResultModel.Create(
                    Uri.AbsoluteUri,
                    response);

                await _navigationService.Navigate<NewsCheckerResultViewModel, NewsCheckerResultModel>(
                    newsCheckerResultModel,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (HttpRequestException)
            {
                var newsCheckerResultModel = NewsCheckerResultModel.IsErrorResultModel(Uri.AbsoluteUri);

                await _navigationService.Navigate<NewsCheckerResultViewModel, NewsCheckerResultModel>(
                    newsCheckerResultModel,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private Task ShowFormatMessageAsync(string message, string title, string ok, CancellationToken cancellationToken)
        {
            return _userDialogs.AlertAsync(message, title: title, okText: ok, cancelToken: cancellationToken);
        }
    }
}
