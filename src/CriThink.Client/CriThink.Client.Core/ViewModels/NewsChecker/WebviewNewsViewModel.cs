using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class WebviewNewsViewModel : BaseViewModel<string>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ILogger<NewsCheckerResultViewModel> _logger;
        public WebviewNewsViewModel(ILogger<NewsCheckerResultViewModel> logger, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _logger = logger;
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
    }
}
