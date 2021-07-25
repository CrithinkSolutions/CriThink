using System;
using CriThink.Common.Endpoints.DTOs.Admin;
using Microsoft.Extensions.Logging;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsDetailsViewModel : BaseViewModel<DebunkingNewsGetResponse>
    {
        private readonly ILogger<DebunkingNewsDetailsViewModel> _logger;

        private DebunkingNewsGetResponse _debunkingNewsId;

        public DebunkingNewsDetailsViewModel(ILogger<DebunkingNewsDetailsViewModel> logger)
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

        public override void Prepare(DebunkingNewsGetResponse parameter)
        {
            if (parameter is null)
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _logger?.LogError(argumentNullException, "The given debunking news id is null");
                throw argumentNullException;
            }

            _debunkingNewsId = parameter;
            Uri = new Uri(parameter.NewsLink);
            Title = parameter.Title;
            _logger?.LogInformation("Visit debunking news details", _debunkingNewsId);
        }
    }
}
