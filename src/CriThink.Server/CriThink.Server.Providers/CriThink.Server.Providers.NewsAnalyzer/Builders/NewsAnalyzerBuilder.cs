using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;
using CriThink.Server.Providers.NewsAnalyzer.Analyzers;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.NewsAnalyzer.Builders
{
    public class NewsAnalyzerBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<Task<NewsAnalysisProviderResult>> _queue;

        private bool _isOrtographicCheckEnabled;
        private bool _isTextSentimentAnalysisEnabled;
        private NewsScraperProviderResponse _scrapedNews;

        private IAnalyzer<NewsAnalysisProviderResult> _analyzer;

        public NewsAnalyzerBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _queue = new ConcurrentQueue<Task<NewsAnalysisProviderResult>>();
        }

        public NewsAnalyzerBuilder SetScrapedNews(NewsScraperProviderResponse scrapedNews)
        {
            _scrapedNews = scrapedNews ?? throw new ArgumentNullException(nameof(scrapedNews));
            return this;
        }

        public NewsAnalyzerBuilder EnableOrtographicCheck(bool enabled = true)
        {
            _isOrtographicCheckEnabled = enabled;
            return this;
        }

        public NewsAnalyzerBuilder EnabledTextSentimentAnalysis(bool enabled = true)
        {
            _isTextSentimentAnalysisEnabled = enabled;
            return this;
        }

        internal IAnalyzer<NewsAnalysisProviderResult> BuildAnalyzers()
        {
            _queue.Clear();

            if (_isOrtographicCheckEnabled)
            {
                var languageAnalyzer = GetAnalyzerService<LanguageAnalyzer>();
                AddAnalyzer(languageAnalyzer);
            }

            if (_isTextSentimentAnalysisEnabled)
            {
                var textSentimentAnalyzer = GetAnalyzerService<TextSentimentAnalyzer>();
                AddAnalyzer(textSentimentAnalyzer);
            }

            return _analyzer;
        }

        private BaseNewsAnalyzer GetAnalyzerService<T>() where T : BaseNewsAnalyzer
        {
            var analyzerService = _serviceProvider.GetRequiredService<T>();
            analyzerService.ScrapedNews = _scrapedNews;
            analyzerService.Queue = _queue;

            return analyzerService;
        }

        private void AddAnalyzer(IAnalyzer<NewsAnalysisProviderResult> analyzer)
        {
            if (_analyzer == null)
                _analyzer = analyzer;
            else
                _analyzer.SetNext(analyzer);
        }
    }
}
