using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using MvvmCross.Commands;
using MvvmCross.Logging;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class RelatedDebunkingNewsViewModel : BaseViewModel
    {
        private readonly IDebunkingNewsService _debunkingNewsService;
        private readonly IMvxLog _log;

        private string _newsId;
        private string _newsLink;

        public RelatedDebunkingNewsViewModel(IDebunkingNewsService debunkingNewsService, IMvxLogProvider logProvider)
        {
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
            _log = logProvider?.GetLogFor<RelatedDebunkingNewsViewModel>();
        }

        #region Properties

        private NewsSourceRelatedDebunkingNewsResponse _debunkingNews;

        public NewsSourceRelatedDebunkingNewsResponse DebunkingNews
        {
            get => _debunkingNews;
            set
            {
                _debunkingNews = value;

                if (_debunkingNews is null)
                    return;

                _newsId = _debunkingNews.Id;
                _newsLink = _debunkingNews.NewsLink;
                Title = _debunkingNews.Title;
                Publisher = _debunkingNews.Publisher;
                NewsImageLink = _debunkingNews.NewsImageLink;
                Caption = _debunkingNews.Caption;
                PublishingDate = _debunkingNews.PublishingDate;
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _publisher;
        public string Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value);
        }

        private string _newsImageLink;
        public string NewsImageLink
        {
            get => _newsImageLink;
            set => SetProperty(ref _newsImageLink, value);
        }

        private string _caption;
        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value);
        }

        private string _publishingDate;
        public string PublishingDate
        {
            get => _publishingDate;
            set => SetProperty(ref _publishingDate, value);
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _openDebunkingNewsCommand;
        public IMvxAsyncCommand OpenDebunkingNewsCommand => _openDebunkingNewsCommand ??= new MvxAsyncCommand(DoOpenDebunkingNewsCommand);

        #endregion

        private async Task DoOpenDebunkingNewsCommand()
        {
            if (string.IsNullOrWhiteSpace(_newsLink))
                return;

            await _debunkingNewsService.OpenDebunkingNewsInBrowser(_newsLink).ConfigureAwait(false);
            _log?.Info("User opens related debunking news", _newsLink);
        }
    }
}
