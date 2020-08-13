﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal class LanguageAnalyzer : BaseNewsAnalyzer
    {
        private readonly NewsAnalysisType _analysisType;

        public LanguageAnalyzer(NewsScraperProviderResponse scrapedNews, ConcurrentQueue<Task<NewsAnalysisProviderResult>> queue)
            : base(scrapedNews, queue)
        {
            _analysisType = NewsAnalysisType.Ortographic;
        }

        public override Task<NewsAnalysisProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(() =>
            {
                Debug.WriteLine("Get in LanguageAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private NewsAnalysisProviderResult RunAnalysis()
        {
            return new NewsAnalysisProviderResult(_analysisType, ScrapedNews.RequestedUri, new NotImplementedException("Language analyzer is not implemented yet"));
        }
    }
}