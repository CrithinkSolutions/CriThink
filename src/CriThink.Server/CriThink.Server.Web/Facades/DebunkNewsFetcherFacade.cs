using System;
using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkNewsFetcher;
using CriThink.Server.Providers.DebunkNewsFetcher.Builders;
using CriThink.Server.Providers.DebunkNewsFetcher.Providers;

namespace CriThink.Server.Web.Facades
{
    public class DebunkNewsFetcherFacade : IDebunkNewsFetcherFacade
    {
        private readonly IDebunkNewsProvider _debunkNewsProvider;

        public DebunkNewsFetcherFacade(IDebunkNewsProvider debunkNewsProvider)
        {
            _debunkNewsProvider = debunkNewsProvider ?? throw new ArgumentNullException(nameof(debunkNewsProvider));
        }

        public async Task<DebunkingNewsProviderResult[]> FetchOpenOnlineDebunkNewsAsync()
        {
            var builder = new DebunkingNewsFetcherBuilder()
                .EnableOpenOnline(true);

            var response = await FetchDebunkNewsAsync(builder).ConfigureAwait(false);
            return response;
        }

        private Task<DebunkingNewsProviderResult[]> FetchDebunkNewsAsync(DebunkingNewsFetcherBuilder fetcherBuilder)
        {
            var analyzerTasks = _debunkNewsProvider.StartFetcherAsync(fetcherBuilder);
            return Task.WhenAll(analyzerTasks);
        }
    }

    public interface IDebunkNewsFetcherFacade
    {
        Task<DebunkingNewsProviderResult[]> FetchOpenOnlineDebunkNewsAsync();
    }
}
