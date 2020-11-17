using CriThink.Server.Providers.DomainAnalyzer.Builders;
using CriThink.Server.Providers.DomainAnalyzer.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.DomainAnalyzer
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class DomainAnalyzerBootstrapper
    {
        /// <summary>
        /// Initialize the library
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void AddDomainAnalyzerProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<DomainAnalyzerBuilder>();
            serviceCollection.AddTransient<IDomainAnalyzerProvider, DomainAnalyzerProvider>();
        }
    }
}
