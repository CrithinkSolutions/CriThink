using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.Admin;
using MvvmCross.Logging;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsDetailsViewModel : BaseViewModel<string>, IDisposable
    {
        private readonly IDebunkingNewsService _debunkingNewsService;
        private readonly IMvxLog _log;

        private string _debunkingNewsId;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _disposed;

        public DebunkingNewsDetailsViewModel(IDebunkingNewsService debunkngNewsService, IMvxLogProvider logProvider)
        {
            _debunkingNewsService = debunkngNewsService ?? throw new ArgumentNullException(nameof(debunkngNewsService));
            _log = logProvider?.GetLogFor<DebunkingNewsDetailsViewModel>();
        }

        private DebunkingNewsGetDetailsResponse _debunkingNews;
        public DebunkingNewsGetDetailsResponse DebunkingNews
        {
            get => _debunkingNews;
            set => SetProperty(ref _debunkingNews, value);
        }

        public override void Prepare(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _log?.ErrorException("The given debunking news id is null", argumentNullException);
                throw argumentNullException;
            }

            _debunkingNewsId = parameter;
            _log?.Info("Visit debunking news details", _debunkingNewsId);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var debunkingNews = await _debunkingNewsService.GetDebunkingNewsByIdAsync(_debunkingNewsId, _cancellationTokenSource.Token).ConfigureAwait(true);
            if (debunkingNews != null)
                DebunkingNews = debunkingNews;
        }

        #region Disposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
