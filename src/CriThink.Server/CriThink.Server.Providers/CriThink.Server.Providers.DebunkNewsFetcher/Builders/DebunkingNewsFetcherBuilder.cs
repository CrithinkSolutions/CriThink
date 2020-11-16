using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;
using CriThink.Server.Providers.DebunkNewsFetcher.Fetchers;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Builders
{
    public class DebunkingNewsFetcherBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<Task<DebunkingNewsProviderResult>> _queue;

        private bool _isOpenOnlineEnabled;
        private IAnalyzer<DebunkingNewsProviderResult> _analyzer;

        public DebunkingNewsFetcherBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _queue = new ConcurrentQueue<Task<DebunkingNewsProviderResult>>();
        }

        public DebunkingNewsFetcherBuilder EnableOpenOnline(bool enabled = true)
        {
            _isOpenOnlineEnabled = enabled;
            return this;
        }

        internal IAnalyzer<DebunkingNewsProviderResult> BuildFetchers()
        {
            _queue.Clear();

            if (_isOpenOnlineEnabled)
            {
                var openOnlineFetcher = GetFetcher<OpenOnlineFetcher>();
                AddFetcher(openOnlineFetcher);
            }

            return _analyzer;
        }

        private BaseFetcher GetFetcher<T>() where T : BaseFetcher
        {
            var analyzerService = _serviceProvider.GetRequiredService<T>();
            analyzerService.Queue = _queue;

            return analyzerService;
        }

        private void AddFetcher(IAnalyzer<DebunkingNewsProviderResult> fetcher)
        {
            if (_analyzer == null)
                _analyzer = fetcher;
            else
                _analyzer.SetNext(fetcher);
        }
    }
}
