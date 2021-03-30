using System;
using CriThink.Common.Endpoints.DTOs.Admin;
using MvvmCross.Logging;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsDetailsViewModel : BaseViewModel<DebunkingNewsGetResponse>
    {
        private readonly IMvxLog _log;

        private DebunkingNewsGetResponse _debunkingNewsId;

        public DebunkingNewsDetailsViewModel(IMvxLogProvider logProvider)
        {
            _log = logProvider?.GetLogFor<DebunkingNewsDetailsViewModel>();
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
                _log?.ErrorException("The given debunking news id is null", argumentNullException);
                throw argumentNullException;
            }

            _debunkingNewsId = parameter;
            Uri = new Uri(parameter.NewsLink);
            Title = parameter.Title;
            _log?.Info("Visit debunking news details", _debunkingNewsId);
        }
    }
}
