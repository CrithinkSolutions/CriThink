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

            serviceCollection.AddTransient<OpenOnlineFetcher>();
            serviceCollection.AddTransient<DebunkingNewsFetcherBuilder>();

            serviceCollection.AddTransient<IDebunkNewsProvider, DebunkingNewsProvider>();
        }
    }
}
