using System;
using System.Threading.Tasks;
using CriThink.Server.Providers.DomainAnalyzer;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Core.Facades
{
    public interface IDomainAnalyzerFacade
    {
        /// <summary>
        /// Ask if the given <see cref="Uri" /> has HTTPS support
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="DomainAnalysisProviderResult"/> with analysis results</returns>
        Task<DomainAnalysisProviderResult> HasHttpsSupportAsync(Uri uri);

        /// <summary>
        /// Get the domain info of the given <see cref="Uri"/>
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="DomainAnalysisProviderResult"/> with analysis results</returns>
        Task<DomainAnalysisProviderResult> GetDomainInfoAsync(Uri uri);

        /// <summary>
        /// Perform an analysis of the given <see cref="Uri"/> using all the NewsAnalyzer available
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="DomainAnalysisProviderResult"/> with analysis results</returns>
        Task<DomainAnalysisProviderResult[]> GetCompleteAnalysisAsync(Uri uri);
    }
}
