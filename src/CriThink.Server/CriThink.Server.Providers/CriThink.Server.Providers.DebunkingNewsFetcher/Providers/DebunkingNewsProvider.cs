using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkingNewsFetcher.Builders;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace CriThink.Server.Providers.DebunkingNewsFetcher.Providers
{
    internal class DebunkingNewsProvider : IDebunkingNewsProvider
    {
        public Task<DebunkingNewsProviderResult>[] StartFetcherAsync(
            DebunkingNewsFetcherBuilder fetcherBuilder)
        {
            return fetcherBuilder
                .BuildFetchers()
                .AnalyzeAsync();
        }
    }

    public interface IDebunkingNewsProvider
    {
        /// <summary>
        /// Start the fetchers 
        /// </summary>
        /// <param name="fetcherBuilder">The request containing the list of debunking news publishers</param>
        /// <returns>The fetcher results</returns>
        Task<DebunkingNewsProviderResult>[] StartFetcherAsync(
            DebunkingNewsFetcherBuilder fetcherBuilder);
    }
}
