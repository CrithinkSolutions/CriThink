using CriThink.Server.Providers.NewsAnalyzer.Managers;
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
        public static void AddNewsAnalyzer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INewsScraperManager, NewsScraperManager>();
        }
    }
}
