using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Providers.NewsAnalyzer.Analyzers;

namespace CriThink.Server.Providers.NewsAnalyzer.Builders
{
    public class NewsAnalyzerBuilder
    {
        private readonly ConcurrentQueue<Task<NewsAnalysisProviderResponse>> _queue;

        private bool _isOrtographicCheckEnabled;
        private bool _isTextSentimentAnalysisEnabled;
        private NewsScraperProviderResponse _scrapedNews;

        private INewsAnalyzer _analyzer;

        public NewsAnalyzerBuilder()
        {
            _queue = new ConcurrentQueue<Task<NewsAnalysisProviderResponse>>();
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

        internal INewsAnalyzer BuildAnalyzers()
        {
            _queue.Clear();

            if (_isOrtographicCheckEnabled)
                AddAnalyzer(new LanguageAnalyzer(_scrapedNews, _queue));

            if (_isTextSentimentAnalysisEnabled)
                AddAnalyzer(new TextSentimentAnalyzer(_scrapedNews, _queue));

            return _analyzer;
        }

        private void AddAnalyzer(INewsAnalyzer analyzer)
        {
            if (_analyzer == null)
                _analyzer = analyzer;
            else
                _analyzer.SetNext(analyzer);
        }
    }
}
