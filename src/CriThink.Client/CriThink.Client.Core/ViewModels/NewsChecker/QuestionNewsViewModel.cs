using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class QuestionNewsViewModel : BaseViewModel<IList<NewsSourceGetQuestionResponse>>
    {
        private readonly IMvxNavigationService _navigationService;

        public QuestionNewsViewModel(
            IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            Questions = new MvxObservableCollection<NewsSourceGetQuestionResponse>();
        }

        public MvxObservableCollection<NewsSourceGetQuestionResponse> Questions { get; private set; }

        private IMvxCommand _questionNewsSubmitCommand;
        public IMvxCommand QuestionNewsSubmitCommand =>
            _questionNewsSubmitCommand ??= new MvxAsyncCommand(DoQuestionNewsSubmitCommand);

        public override void Prepare(IList<NewsSourceGetQuestionResponse> parameter)
        {
            Questions = new MvxObservableCollection<NewsSourceGetQuestionResponse>(parameter);
        }

        private async Task DoQuestionNewsSubmitCommand(CancellationToken cancellationToken)
        {
            await _navigationService.Navigate<NewsCheckerResultViewModel, string>(string.Empty, cancellationToken: cancellationToken)
                .ConfigureAwait(true);

        }
    }
}
