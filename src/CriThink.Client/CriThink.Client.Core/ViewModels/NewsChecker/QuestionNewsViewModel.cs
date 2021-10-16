 using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Serilog.Core;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class QuestionNewsViewModel : BaseViewModel<IList<NewsSourceGetQuestionResponse>>
    {
        public MvxObservableCollection<NewsSourceGetQuestionResponse> Questions { get; private set; }
        private readonly ILogger<QuestionNewsViewModel> _logger;
        private readonly IMvxNavigationService _navigationService;

        public QuestionNewsViewModel(
            ILogger<QuestionNewsViewModel> logger,
            IMvxNavigationService navigationService
            )
        {
            Questions = new MvxObservableCollection<NewsSourceGetQuestionResponse>();
            _logger = logger;
            _navigationService = navigationService;
        }


        public override void Prepare(IList<NewsSourceGetQuestionResponse> parameter)
        {
            Questions = new MvxObservableCollection<NewsSourceGetQuestionResponse>(parameter);
        }

    


        

        private IMvxCommand _questionNewsSubmitCommand;
        public IMvxCommand QuestionNewsSubmitCommand =>
            _questionNewsSubmitCommand ??= new MvxAsyncCommand(DoQuestionNewsSubmitCommand);

        private async Task DoQuestionNewsSubmitCommand(CancellationToken cancellationToken)
        {
            
            await _navigationService.Navigate<NewsCheckerResultViewModel, string>(string.Empty, cancellationToken: cancellationToken)
                .ConfigureAwait(true);
            
        }

    }
}
