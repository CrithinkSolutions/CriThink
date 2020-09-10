using CriThink.Server.Providers.DebunkNewsFetcher.Fetchers;
using CriThink.Server.Providers.DebunkNewsFetcher.Providers;
using CriThink.Server.Providers.DebunkNewsFetcher.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.DebunkNewsFetcher
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class DebunkNewsFetcherBootstrapper
    {
        public const string OpenOnlineHttpClientName = "OpenOnlineFeed";

        /// <summary>
        /// Initialize the library
        /// </summary>
        /// <param name="serviceCollection">The IoC container</param>
        /// <param name="openOnline">OpenOnline feed uris with keywords</param>
        public static void AddDebunkNewsFetcherProvider(this IServiceCollection serviceCollection, WebSiteSettings openOnline)
        {
            serviceCollection.AddHttpClient(OpenOnlineHttpClientName, client =>
            {
                OpenOnlineFetcher.FeedCategories.AddRange(openOnline.Categories);
                OpenOnlineFetcher.WebSiteUri = openOnline.Uri;

                //client.BaseAddress = ;
                client.DefaultRequestHeaders.Add("api-version", "1.0");
            });

            serviceCollection.AddTransient<IDebunkNewsProvider, DebunkNewsProvider>();
        }
    }
}
