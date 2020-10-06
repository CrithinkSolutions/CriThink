using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;
using CriThink.Server.Providers.DebunkNewsFetcher.Fetchers;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Builders
{
    public class DebunkingNewsFetcherBuilder
    {
        private readonly ConcurrentQueue<Task<DebunkingNewsProviderResult>> _queue;

        private bool _isOpenOnlineEnabled;
        private IAnalyzer<DebunkingNewsProviderResult> _analyzer;
        private IHttpClientFactory _httpClientFactory;

        public DebunkingNewsFetcherBuilder()
        {
            _queue = new ConcurrentQueue<Task<DebunkingNewsProviderResult>>();
        }

        public DebunkingNewsFetcherBuilder EnableOpenOnline(bool enabled = true)
        {
            _isOpenOnlineEnabled = enabled;
            return this;
        }

        internal DebunkingNewsFetcherBuilder SetHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            return this;
        }

        internal IAnalyzer<DebunkingNewsProviderResult> BuildFetchers()
        {
            _queue.Clear();

            if (_isOpenOnlineEnabled)
                AddFetcher(new OpenOnlineFetcher(_queue, _httpClientFactory));

            return _analyzer;
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
