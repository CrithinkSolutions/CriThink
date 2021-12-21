using System;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using Microsoft.Extensions.Logging;

namespace CriThink.Client.Core.ViewModels.Common
{
    public class CriThinkHtmlViewModel : BaseViewModel<CriThinkHtmlViewData>
    {
        private readonly ILogger<DebunkingNewsDetailsViewModel> _logger;

        public CriThinkHtmlViewModel(ILogger<DebunkingNewsDetailsViewModel> logger)
        {
            _logger = logger;
        }

        #region Properties

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private Uri _uri;
        public Uri Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        #endregion

        public override void Prepare(CriThinkHtmlViewData parameter)
        {
            if (parameter is null)
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _logger?.LogError(argumentNullException, "The given html view data is null");
                throw argumentNullException;
            }

            Uri = parameter.Uri;
            Title = parameter.Title;
            _logger?.LogInformation("Visit web view", parameter.Uri);
        }
    }
}
