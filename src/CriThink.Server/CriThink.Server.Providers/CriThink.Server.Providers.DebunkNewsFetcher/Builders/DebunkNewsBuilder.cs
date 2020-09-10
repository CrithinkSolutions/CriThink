using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;
using CriThink.Server.Providers.DebunkNewsFetcher.Fetchers;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Builders
{
    public class DebunkNewsBuilder
    {
        private readonly ConcurrentQueue<Task<DebunkNewsProviderResult>> _queue;

        private bool _isOpenOnlineEnabled;
        private IAnalyzer<DebunkNewsProviderResult> _analyzer;
        private IHttpClientFactory _httpClientFactory;

        public DebunkNewsBuilder()
        {
            _queue = new ConcurrentQueue<Task<DebunkNewsProviderResult>>();
        }

        public DebunkNewsBuilder EnableOpenOnline(bool enabled = true)
        {
            _isOpenOnlineEnabled = enabled;
            return this;
        }

        internal DebunkNewsBuilder SetHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            return this;
        }

        internal IAnalyzer<DebunkNewsProviderResult> BuildFetchers()
        {
            _queue.Clear();

            if (_isOpenOnlineEnabled)
                AddFetcher(new OpenOnlineFetcher(_queue, _httpClientFactory));

            return _analyzer;
        }

        private void AddFetcher(IAnalyzer<DebunkNewsProviderResult> fetcher)
        {
            if (_analyzer == null)
                _analyzer = fetcher;
            else
                _analyzer.SetNext(fetcher);
        }
    }
}
