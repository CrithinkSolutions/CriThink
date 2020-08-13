using System.Threading.Tasks;

namespace CriThink.Server.Core.Providers
{
    /// <summary>
    /// Provides a common interface for the providers' analyzers chains
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAnalyzer<T> where T : IProviderResult
    {
        /// <summary>
        /// Add the next <see cref="IAnalyzer{T}"/> to the chain
        /// </summary>
        /// <param name="analyzer"><see cref="IAnalyzer{T}"/> instance</param>
        /// <returns>Returns the given <see cref="IAnalyzer{T}"/> instance</returns>
        IAnalyzer<T> SetNext(IAnalyzer<T> analyzer);

        /// <summary>
        /// Start the analyzers
        /// </summary>
        /// <returns>Returns the list of analysis results</returns>
        Task<T>[] AnalyzeAsync();
    }
}
