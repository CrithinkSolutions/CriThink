using System.Threading.Tasks;
using CriThink.Server.Providers.NewsAnalyzer.Builders;

namespace CriThink.Server.Providers.NewsAnalyzer.Providers
{
    internal class NewsAnalyzerProvider : INewsAnalyzerProvider
    {
        public Task<NewsAnalysisProviderResult>[] StartAnalyzerAsync(NewsAnalyzerBuilder builder)
        {
            return builder
                .BuildAnalyzers()
                .AnalyzeAsync();
        }
    }

    public interface INewsAnalyzerProvider
    {
        /// <summary>
        /// Start the URLs analysis
        /// </summary>
        /// <param name="builder">The request containing the kind of analysis to perform</param>
        /// <returns>The analysis results</returns>
        Task<NewsAnalysisProviderResult>[] StartAnalyzerAsync(NewsAnalyzerBuilder builder);
    }
}
