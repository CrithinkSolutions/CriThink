using System.Threading.Tasks;
using CriThink.Server.Providers.NewsAnalyzer;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Core.Facades
{
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
