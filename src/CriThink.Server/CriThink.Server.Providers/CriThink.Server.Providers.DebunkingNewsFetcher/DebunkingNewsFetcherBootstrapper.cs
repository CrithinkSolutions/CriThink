using System.Net.Http;
using CriThink.Server.Providers.DebunkingNewsFetcher.Builders;
using CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers;
using CriThink.Server.Providers.DebunkingNewsFetcher.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.DebunkingNewsFetcher
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class DebunkingNewsFetcherBootstrapper
    {
        public const string OpenOnlineHttpClientName = "OpenOnlineFeed";
        public const string Channel4HttpClientName = "Channel4";
        public const string FactaNewsHttpClientName = "FactaNews";
        public const string UrlResolverHttpClientName = "UrlResolver";

        /// <summary>
        /// Initialize the library
        /// </summary>
        /// <param name="serviceCollection">The IoC container</param>
        public static void AddDebunkNewsFetcherProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient(OpenOnlineHttpClientName);

            serviceCollection.AddHttpClient(Channel4HttpClientName);

            serviceCollection.AddHttpClient(FactaNewsHttpClientName);

            serviceCollection.AddHttpClient(UrlResolverHttpClientName)
                             .ConfigurePrimaryHttpMessageHandler(()
                              => new HttpClientHandler
                              {
                                  AllowAutoRedirect = false,
                              });

            serviceCollection.AddTransient<OpenOnlineFetcher>();
            serviceCollection.AddTransient<Channel4Fetcher>();
            serviceCollection.AddTransient<FactaNewsFetcher>();
            serviceCollection.AddTransient<DebunkingNewsFetcherBuilder>();

            serviceCollection.AddTransient<IDebunkingNewsProvider, DebunkingNewsProvider>();
        }
    }
}
