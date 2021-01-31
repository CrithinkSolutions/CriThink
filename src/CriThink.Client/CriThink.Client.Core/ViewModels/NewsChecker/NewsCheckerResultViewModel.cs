using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using MvvmCross.Logging;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerResultViewModel : BaseViewModel<Uri>, IDisposable
    {
        private readonly INewsSourceService _newsSourceService;
        private readonly IMvxLog _log;

        private Uri _uri;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _disposed;

        public NewsCheckerResultViewModel(INewsSourceService newsSourceService, IMvxLogProvider logProvider)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _log = logProvider?.GetLogFor<NewsCheckerResultViewModel>();
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _classification;

        public string Classification
        {
            get => _classification;
            set => SetProperty(ref _classification, value);
        }

        public override void Prepare(Uri parameter)
        {
            if (parameter == null)
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _log?.FatalException("The given paramter is null", argumentNullException);
                throw argumentNullException;
            }

            _uri = parameter;
            _log?.Info("User checks news source", _uri);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var result = await _newsSourceService.SearchNewsSourceAsync(_uri, _cancellationTokenSource.Token).ConfigureAwait(false);
            if (result is null)
                return;

            Description = result.Description;
            Classification = result.Classification.ToString();
        }

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
    }
}
