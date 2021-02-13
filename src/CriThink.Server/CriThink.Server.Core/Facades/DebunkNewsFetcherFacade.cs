using System;
using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkNewsFetcher;
using CriThink.Server.Providers.DebunkNewsFetcher.Builders;
using CriThink.Server.Providers.DebunkNewsFetcher.Providers;

namespace CriThink.Server.Core.Facades
{
    public class DebunkNewsFetcherFacade : IDebunkNewsFetcherFacade
    {
        private readonly DebunkingNewsFetcherBuilder _builder;
        private readonly IDebunkNewsProvider _debunkNewsProvider;

        public DebunkNewsFetcherFacade(IDebunkNewsProvider debunkNewsProvider, DebunkingNewsFetcherBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _debunkNewsProvider = debunkNewsProvider ?? throw new ArgumentNullException(nameof(debunkNewsProvider));
        }

        public async Task<DebunkingNewsProviderResult[]> FetchDebunkingNewsAsync()
        {
            var builder = _builder
                .EnableOpenOnline(true)
                .EnableChannel4(true);

            var response = await FetchDebunkNewsAsync(builder).ConfigureAwait(false);
            return response;
        }

        private Task<DebunkingNewsProviderResult[]> FetchDebunkNewsAsync(DebunkingNewsFetcherBuilder fetcherBuilder)
        {
            var analyzerTasks = _debunkNewsProvider.StartFetcherAsync(fetcherBuilder);
            return Task.WhenAll(analyzerTasks);
        }
    }
}
