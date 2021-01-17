using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.Admin;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsDetailsViewModel : BaseViewModel<string>, IDisposable
    {
        private readonly IDebunkingNewsService _debunkingNewsService;

        private string _debunkingNewsId;
        private CancellationTokenSource _cancellationTokenSource;

        private bool disposed;

        public DebunkingNewsDetailsViewModel(IDebunkingNewsService debunkngNewsService)
        {
            _debunkingNewsService = debunkngNewsService ?? throw new ArgumentNullException(nameof(debunkngNewsService));
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
                throw new ArgumentNullException(nameof(parameter));

            _debunkingNewsId = parameter;
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            DebunkingNews = await _debunkingNewsService.GetDebunkingNewsByIdAsync(_debunkingNewsId, _cancellationTokenSource.Token).ConfigureAwait(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _cancellationTokenSource?.Dispose();
                }

                disposed = true;
            }
        }
    }
}
