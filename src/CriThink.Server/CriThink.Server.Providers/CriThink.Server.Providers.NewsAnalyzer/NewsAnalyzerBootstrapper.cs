using System;
using Azure;
using Azure.AI.TextAnalytics;
using CriThink.Server.Providers.NewsAnalyzer.Analyzers;
using CriThink.Server.Providers.NewsAnalyzer.Builders;
using CriThink.Server.Providers.NewsAnalyzer.Features;
using CriThink.Server.Providers.NewsAnalyzer.Providers;
using CriThink.Server.Providers.NewsAnalyzer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

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
            serviceCollection.AddTransient<TextSentimentAnalyzer>();
            serviceCollection.AddTransient<LanguageAnalyzer>();
            serviceCollection.AddTransient<NewsAnalyzerBuilder>();
            serviceCollection.AddTransient<INewsAnalyzerProvider, NewsAnalyzerProvider>();

            serviceCollection.AddScoped(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var azureEndpoint = configuration["Azure-Cognitive-Endpoint"];
                var azureCredentials = configuration["Azure-Cognitive-KeyCredentials"];

                return new TextAnalyticsClient(
                    new Uri(azureEndpoint),
                    new AzureKeyCredential(azureCredentials));
            });

            // Services
            serviceCollection.AddScoped<INewsScraperService, NewsScraperService>();
            serviceCollection.AddScoped<ITextAnalyticsService>((sp) =>
            {
                var featureManager = sp.GetRequiredService<IFeatureManager>();
                var isEnabled = featureManager.IsEnabledAsync(nameof(FeatureFlags.TextAnalytics)).Result;
                if (isEnabled)
                    return new TextAnalyticsService(
                        sp.GetRequiredService<TextAnalyticsClient>(),
                        sp.GetService<ILogger<TextAnalyticsService>>());

                return new EmptyTextAnalyticsService();
            });
        }
    }
}
