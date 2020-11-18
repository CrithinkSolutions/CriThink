using System;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkNewsFetcher.Builders;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Providers
{
    internal class DebunkingNewsProvider : IDebunkNewsProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DebunkingNewsProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public Task<DebunkingNewsProviderResult>[] StartFetcherAsync(DebunkingNewsFetcherBuilder fetcherBuilder)
        {
            return fetcherBuilder
                .BuildFetchers()
                .AnalyzeAsync();
        }
    }

    public interface IDebunkNewsProvider
    {
        /// <summary>
        /// Start the fetchers 
        /// </summary>
        /// <param name="fetcherBuilder">The request containing the list of debunking news publishers</param>
        /// <returns>The fetcher results</returns>
        Task<DebunkingNewsProviderResult>[] StartFetcherAsync(DebunkingNewsFetcherBuilder fetcherBuilder);
    }
}
