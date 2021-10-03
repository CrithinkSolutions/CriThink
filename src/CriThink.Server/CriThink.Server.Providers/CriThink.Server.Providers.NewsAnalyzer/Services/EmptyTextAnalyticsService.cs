using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.NewsAnalyzer.Services
{
    internal class EmptyTextAnalyticsService : ITextAnalyticsService
    {
        public Task<IReadOnlyList<string>> GetKeywordsFromTextAsync(string text, string language)
        {
            IReadOnlyList<string> result = new List<string>();
            return Task.FromResult(result);
        }
    }
}
