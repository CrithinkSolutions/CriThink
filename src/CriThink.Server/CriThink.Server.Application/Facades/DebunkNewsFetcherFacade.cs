using System;
using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkingNewsFetcher;
using CriThink.Server.Providers.DebunkingNewsFetcher.Builders;
using CriThink.Server.Providers.DebunkingNewsFetcher.Providers;

namespace CriThink.Server.Application.Facades
{
    public class DebunkNewsFetcherFacade : IDebunkNewsFetcherFacade
    {
        private readonly DebunkingNewsFetcherBuilder _builder;
        private readonly IDebunkingNewsProvider _debunkingNewsProvider;

        public DebunkNewsFetcherFacade(IDebunkingNewsProvider debunkingNewsProvider, DebunkingNewsFetcherBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _debunkingNewsProvider = debunkingNewsProvider ?? throw new ArgumentNullException(nameof(debunkingNewsProvider));
        }

        public async Task<DebunkingNewsProviderResult[]> FetchDebunkingNewsAsync(DateTime lastFetchingTimeStamp)
        {
            var builder = _builder
                .EnableTimeStampCap(lastFetchingTimeStamp)
                .EnableOpenOnline(true)
                .EnableChannel4(true)
                .EnableFullFact(true)
                .EnableFactaNews(true);

            var response = await FetchDebunkNewsAsync(builder);
            return response;
        }

        private Task<DebunkingNewsProviderResult[]> FetchDebunkNewsAsync(
            DebunkingNewsFetcherBuilder fetcherBuilder)
        {
            var analyzerTasks = _debunkingNewsProvider.StartFetcherAsync(fetcherBuilder);
            return Task.WhenAll(analyzerTasks);
        }
    }
}
