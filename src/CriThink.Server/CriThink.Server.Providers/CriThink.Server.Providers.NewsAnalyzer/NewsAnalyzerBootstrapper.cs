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
        /// <param name="azureCredentials">The Azure Cognitive Services credentials</param>
        /// <param name="azureEndpoint">The Azure Cognitive Services endpoint</param>
        public static void AddNewsAnalyzerProvider(this IServiceCollection serviceCollection, string azureCredentials, string azureEndpoint)
        {
            NewsAnalyticsClient.SetupClient(azureEndpoint, azureCredentials);

            serviceCollection.AddTransient<INewsAnalyzerProvider, NewsAnalyzerProvider>();
            serviceCollection.AddTransient<INewsScraperManager, NewsScraperManager>();
        }
    }
}
