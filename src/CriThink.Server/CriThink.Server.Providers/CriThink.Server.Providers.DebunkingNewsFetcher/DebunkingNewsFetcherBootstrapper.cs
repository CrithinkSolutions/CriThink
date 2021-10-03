using System;
using System.Net.Http;
using CriThink.Server.Providers.DebunkingNewsFetcher.Builders;
using CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers;
using CriThink.Server.Providers.DebunkingNewsFetcher.Providers;
using CriThink.Server.Providers.NewsAnalyzer;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace CriThink.Server.Providers.DebunkingNewsFetcher
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class DebunkingNewsFetcherBootstrapper
    {
        public const string OpenOnlineHttpClientName = "OpenOnlineFeed";
        public const string Channel4HttpClientName = "Channel4";
        public const string FullFactHttpClientName = "FullFact";
        public const string FactaNewsHttpClientName = "FactaNews";
        public const string UrlResolverHttpClientName = "UrlResolver";

        /// <summary>
        /// Initialize the library
        /// </summary>
        /// <param name="serviceCollection">The IoC container</param>
        public static void AddDebunkNewsFetcherProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddNewsAnalyzerProvider();

            serviceCollection.
                AddHttpClient(OpenOnlineHttpClientName)
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); ;

            serviceCollection
                .AddHttpClient(Channel4HttpClientName)
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            serviceCollection
                .AddHttpClient(FullFactHttpClientName)
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            serviceCollection
                .AddHttpClient(FactaNewsHttpClientName)
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            serviceCollection.AddHttpClient(UrlResolverHttpClientName)
                             .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                             {
                                 AllowAutoRedirect = false,
                             })
                             .AddTransientHttpErrorPolicy(builder =>
                                builder.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            serviceCollection.AddTransient<OpenOnlineFetcher>();
            serviceCollection.AddTransient<Channel4Fetcher>();
            serviceCollection.AddTransient<FullFactFetcher>();
            serviceCollection.AddTransient<FactaNewsFetcher>();
            serviceCollection.AddTransient<DebunkingNewsFetcherBuilder>();

            serviceCollection.AddTransient<IDebunkingNewsProvider, DebunkingNewsProvider>();
        }
    }
}
