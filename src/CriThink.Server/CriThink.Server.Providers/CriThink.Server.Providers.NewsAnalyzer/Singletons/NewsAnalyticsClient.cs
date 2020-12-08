using System;
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace CriThink.Server.Providers.NewsAnalyzer.Singletons
{
    /// <summary>
    /// Singleton to share an instance of the Azure <see cref="TextAnalyticsClient"/>
    /// </summary>
    internal class NewsAnalyticsClient
    {
        public NewsAnalyticsClient(IConfiguration configuration)

        {
            var azureEndpoint = configuration["Azure-Cognitive-Endpoint"];
            var azureCredentials = configuration["Azure-Cognitive-KeyCredentials"];
            Instance ??= new TextAnalyticsClient(new Uri(azureEndpoint), new AzureKeyCredential(azureCredentials));
        }

        public TextAnalyticsClient Instance { get; }
    }
}
