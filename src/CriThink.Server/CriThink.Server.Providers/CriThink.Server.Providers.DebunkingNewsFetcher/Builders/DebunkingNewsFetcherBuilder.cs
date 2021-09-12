using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;
using CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Builders
{
    public class DebunkingNewsFetcherBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<Task<DebunkingNewsProviderResult>> _queue;

        private bool _isOpenOnlineEnabled;
        private bool _isChannel4Enabled;
        private bool _isFullFactEnabled;
        private bool _isFactaNewsEnabled;
        private DateTime? _lastFetchingTimeStamp;
        private IAnalyzer<DebunkingNewsProviderResult> _analyzer;

        public DebunkingNewsFetcherBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _queue = new ConcurrentQueue<Task<DebunkingNewsProviderResult>>();
        }

        public DebunkingNewsFetcherBuilder EnableTimeStampCap(DateTime dateTime)
        {
            _lastFetchingTimeStamp = dateTime;
            return this;
        }

        public DebunkingNewsFetcherBuilder EnableOpenOnline(bool enabled = true)
        {
            _isOpenOnlineEnabled = enabled;
            return this;
        }

        public DebunkingNewsFetcherBuilder EnableChannel4(bool enabled = true)
        {
            _isChannel4Enabled = enabled;
            return this;
        }

        public DebunkingNewsFetcherBuilder EnableFullFact(bool enabled = true)
        {
            _isFullFactEnabled = enabled;
            return this;
        }

        public DebunkingNewsFetcherBuilder EnableFactaNews(bool enabled = true)
        {
            _isFactaNewsEnabled = enabled;
            return this;
        }

        internal IAnalyzer<DebunkingNewsProviderResult> BuildFetchers()
        {
            _queue.Clear();

            var fetchers = new List<IAnalyzer<DebunkingNewsProviderResult>>();

            if (_isOpenOnlineEnabled)
            {
                var openOnlineFetcher = GetFetcher<OpenOnlineFetcher>();
                fetchers.Add(openOnlineFetcher);
            }

            if (_isChannel4Enabled)
            {
                var channel4Fetcher = GetFetcher<Channel4Fetcher>();
                fetchers.Add(channel4Fetcher);
            }

            if (_isFullFactEnabled)
            {
                var fullFactFetcher = GetFetcher<FullFactFetcher>();
                fetchers.Add(fullFactFetcher);
            }

            if (_isFactaNewsEnabled)
            {
                var factaFetcher = GetFetcher<FactaNewsFetcher>();
                fetchers.Add(factaFetcher);
            }

            AddFetchers(fetchers);

            return _analyzer;
        }

        private BaseFetcher GetFetcher<T>() where T : BaseFetcher
        {
            var analyzerService = _serviceProvider.GetRequiredService<T>();
            analyzerService.SetQueue(_queue);

            if (_lastFetchingTimeStamp.HasValue)
                analyzerService.SetLastFetchingTimeStamp(_lastFetchingTimeStamp.Value);

            return analyzerService;
        }

        private void AddFetchers(IList<IAnalyzer<DebunkingNewsProviderResult>> fetchers)
        {
            for (var i = 0; i < fetchers.Count; i++)
            {
                var fetcher = fetchers[i];

                if (_analyzer is null)
                {
                    _analyzer = fetcher;
                }
                else
                {
                    fetchers[i - 1]
                        .SetNext(fetcher);
                }
            }
        }
    }
}
