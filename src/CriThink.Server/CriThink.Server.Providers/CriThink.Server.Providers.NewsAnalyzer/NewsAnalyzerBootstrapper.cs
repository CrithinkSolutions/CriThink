using CriThink.Server.Providers.NewsAnalyzer.Analyzers;
using CriThink.Server.Providers.NewsAnalyzer.Builders;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using CriThink.Server.Providers.NewsAnalyzer.Providers;
using CriThink.Server.Providers.NewsAnalyzer.Singletons;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.NewsAnalyzer
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class NewsAnalyzerBootstrapper
    {
        /// <summary>
        /// Initialize the library
        /// </summary>
        /// <param name="serviceCollection">The IoC container</param>
        public static void AddNewsAnalyzerProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<NewsAnalyticsClient>();

            serviceCollection.AddTransient<TextSentimentAnalyzer>();
            serviceCollection.AddTransient<LanguageAnalyzer>();
            serviceCollection.AddTransient<NewsAnalyzerBuilder>();
            serviceCollection.AddTransient<INewsAnalyzerProvider, NewsAnalyzerProvider>();
            serviceCollection.AddTransient<INewsScraperManager, NewsScraperManager>();
        }
    }
}
