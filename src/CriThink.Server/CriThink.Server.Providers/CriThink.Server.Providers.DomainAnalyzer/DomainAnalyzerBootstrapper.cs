using System;
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
        public static void Bootstrap(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddTransient<IDomainAnalyzerProvider, DomainAnalyzerProvider>();
        }
    }
}
