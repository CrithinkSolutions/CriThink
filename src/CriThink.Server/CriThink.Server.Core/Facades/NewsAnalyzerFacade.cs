using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Builders;
using CriThink.Server.Providers.NewsAnalyzer.Providers;

namespace CriThink.Server.Core.Facades
{
    public class NewsAnalyzerFacade : INewsAnalyzerFacade
    {
        private readonly INewsAnalyzerProvider _newsAnalyzerProvider;
        private readonly NewsAnalyzerBuilder _builder;

        public NewsAnalyzerFacade(INewsAnalyzerProvider newsAnalyzerProvider, NewsAnalyzerBuilder builder)
        {
            _newsAnalyzerProvider = newsAnalyzerProvider ?? throw new ArgumentNullException(nameof(newsAnalyzerProvider));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public async Task<NewsAnalysisProviderResult> GetNewsSentimentAsync(NewsScraperProviderResponse scraperResponse)
        {
            var builder = _builder
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
}
