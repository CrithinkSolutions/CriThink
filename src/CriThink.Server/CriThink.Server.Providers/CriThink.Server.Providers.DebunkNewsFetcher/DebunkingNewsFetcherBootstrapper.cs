using System.Net.Http;
using CriThink.Server.Providers.DebunkNewsFetcher.Builders;
using CriThink.Server.Providers.DebunkNewsFetcher.Fetchers;
using CriThink.Server.Providers.DebunkNewsFetcher.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.DebunkNewsFetcher
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class DebunkingNewsFetcherBootstrapper
    {
        public const string OpenOnlineHttpClientName = "OpenOnlineFeed";
        public const string Channel4HttpClientName = "Channel4";
        public const string UrlResolverHttpClientName = "UrlResolver";

        /// <summary>
        /// Initialize the library
        /// </summary>
        /// <param name="serviceCollection">The IoC container</param>
        public static void AddDebunkNewsFetcherProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient(OpenOnlineHttpClientName, client =>
            {
                client.DefaultRequestHeaders.Add("api-version", "1.0");
            });

            serviceCollection.AddHttpClient(Channel4HttpClientName);

            serviceCollection.AddHttpClient(UrlResolverHttpClientName)
                             .ConfigurePrimaryHttpMessageHandler(()
                              => new HttpClientHandler
                              {
                                  AllowAutoRedirect = false,
                              });

            serviceCollection.AddTransient<OpenOnlineFetcher>();
            serviceCollection.AddTransient<Channel4Fetcher>();
            serviceCollection.AddTransient<DebunkingNewsFetcherBuilder>();

            serviceCollection.AddTransient<IDebunkNewsProvider, DebunkingNewsProvider>();
        }
    }
}
