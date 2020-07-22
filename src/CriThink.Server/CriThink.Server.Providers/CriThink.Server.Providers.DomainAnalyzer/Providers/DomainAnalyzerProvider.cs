using System.Threading.Tasks;
using CriThink.Server.Providers.DomainAnalyzer.Builders;

namespace CriThink.Server.Providers.DomainAnalyzer.Providers
{
    internal class DomainAnalyzerProvider : IDomainAnalyzerProvider
    {
        public Task<AnalysisResponse>[] StartAnalyzer(DomainAnalyzerBuilder builder)
        {
            return builder
                .BuildAnalyzers()
                .AnalyzeAsync();
        }
    }

    public interface IDomainAnalyzerProvider
    {
        /// <summary>
        /// Start the URLs analysis
        /// </summary>
        /// <param name="builder">The request containing the kind of analysis to perform</param>
        /// <returns>The analysis results</returns>
        Task<AnalysisResponse>[] StartAnalyzer(DomainAnalyzerBuilder builder);
    }
}
