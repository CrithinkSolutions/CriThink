using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Builders;
using CriThink.Server.Providers.NewsAnalyzer.Providers;

namespace CriThink.Server.Web.Facades
{
    public class NewsAnalyzerFacade : INewsAnalyzerFacade
    {
        private readonly INewsAnalyzerProvider _newsAnalyzerProvider;

        public NewsAnalyzerFacade(INewsAnalyzerProvider newsAnalyzerProvider)
        {
            _newsAnalyzerProvider = newsAnalyzerProvider ?? throw new ArgumentNullException(nameof(newsAnalyzerProvider));
        }

        public async Task<NewsAnalysisProviderResult> GetNewsSentimentAsync(NewsScraperProviderResponse scraperResponse)
        {
            var builder = new NewsAnalyzerBuilder()
                .EnabledTextSentimentAnalysis()
                .SetScrapedNews(scraperResponse);

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses.FirstOrDefault(r => r.NewsAnalysisType == NewsAnalysisType.TextSentiment);
        }

        private Task<NewsAnalysisProviderResult[]> MakeRequestAsync(NewsAnalyzerBuilder builder)
        {
            var analyzerTasks = _newsAnalyzerProvider.StartAnalyzerAsync(builder);
            return Task.WhenAll(analyzerTasks);
        }
    }

    public interface INewsAnalyzerFacade
    {
        /// <summary>
        /// Analyze the sentiment of the given text
        /// </summary>
        /// <param name="scraperResponse">News info</param>
        /// <returns>Analysis results</returns>
        Task<NewsAnalysisProviderResult> GetNewsSentimentAsync(NewsScraperProviderResponse scraperResponse);
    }
}
