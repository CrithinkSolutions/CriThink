using System;
using Azure;
using Azure.AI.TextAnalytics;

namespace CriThink.Server.Providers.NewsAnalyzer.Singletons
{
    /// <summary>
    /// Singleton implementation to share a single instance of the Azure <see cref="TextAnalyticsClient"/>
    /// </summary>
    internal static class NewsAnalyticsClient
    {
        private static string _azureEndpoint;
        private static string _azureCredentials;
        private static TextAnalyticsClient _client;

        private static readonly object Locker = new object();

        public static TextAnalyticsClient Instance
        {
            get
            {
                if (_client == null)
                {
                    lock (Locker)
                    {
                        _client ??= new TextAnalyticsClient(new Uri(_azureEndpoint), new AzureKeyCredential(_azureCredentials));
                    }
                }

                return _client;
            }
        }

        public static void SetupClient(string azureEndpoint, string azureCredentials)
        {
            _azureEndpoint = azureEndpoint;
            _azureCredentials = azureCredentials;
        }
    }
}
