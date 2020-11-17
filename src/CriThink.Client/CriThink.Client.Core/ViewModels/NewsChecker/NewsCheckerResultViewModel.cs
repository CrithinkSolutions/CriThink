using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerResultViewModel : MvxViewModel<Uri>
    {
        private readonly INewsSourceService _newsSourceService;
        private Uri _uri;
        private CancellationTokenSource _cancellationTokenSource;

        public NewsCheckerResultViewModel(INewsSourceService newsSourceService)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
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
                throw new ArgumentNullException(nameof(parameter));

            _uri = parameter;
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var result = await _newsSourceService.SearchNewsSourceAsync(_uri, _cancellationTokenSource.Token).ConfigureAwait(false);

            Description = result.Description;
            Classification = result.Classification.ToString();
        }
    }
}
