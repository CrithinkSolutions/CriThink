using System;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkNewsFetcher.Builders;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Providers
{
    internal class DebunkNewsProvider : IDebunkNewsProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DebunkNewsProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public Task<DebunkNewsProviderResult>[] StartFetcherAsync(DebunkNewsBuilder builder)
        {
            return builder
                .SetHttpClient(_httpClientFactory)
                .BuildFetchers()
                .AnalyzeAsync();
        }
    }

    public interface IDebunkNewsProvider
    {
        /// <summary>
        /// Start the fetchers 
        /// </summary>
        /// <param name="builder">The request containing the list of debunking news publishers</param>
        /// <returns>The fetcher results</returns>
        Task<DebunkNewsProviderResult>[] StartFetcherAsync(DebunkNewsBuilder builder);
    }
}
