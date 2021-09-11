using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.AI.TextAnalytics;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Providers.NewsAnalyzer.Services
{
    internal class TextAnalyticsService : ITextAnalyticsService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly ILogger<TextAnalyticsService> _logger;

        public TextAnalyticsService(
            TextAnalyticsClient textAnalyticsClient,
            ILogger<TextAnalyticsService> logger)
        {
            _textAnalyticsClient = textAnalyticsClient ??
                throw new ArgumentNullException(nameof(textAnalyticsClient));

            _logger = logger;
        }

        public async Task<IReadOnlyList<string>> GetKeywordsFromTextAsync(string text, string language)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            var allKeywords = new List<string>();

            var bodyParts = text.SplitInParts(5120).ToList(); // max lenght supported by Azure

            foreach (var bodyPart in bodyParts)
            {
                try
                {
                    var keywords = await SendAnalysisRequestAsync(bodyPart.ToString(), language);
                    allKeywords.AddRange(keywords);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"Error getting keywords from news '{text}': {bodyPart}");
                }
            }

            return allKeywords
                .Distinct()
                .ToList()
                .AsReadOnly();
        }

        private async Task<IReadOnlyList<string>> SendAnalysisRequestAsync(string newsBody, string language)
        {
            var entities = await _textAnalyticsClient.RecognizeEntitiesAsync(newsBody, language);

            return entities.Value
                .Where(e => e.Category == EntityCategory.Location ||
                            e.Category == EntityCategory.Person ||
                            e.Category == EntityCategory.Product ||
                            e.Category == EntityCategory.Organization &&
                            e.ConfidenceScore > 0.20)
                .Select(c => c.Text)
                .ToList()
                .AsReadOnly();
        }
    }
}
