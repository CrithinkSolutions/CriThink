using System.Threading;
using System.Threading.Tasks;

namespace CriThink.Client.Core.Repositories
{
    /// <summary>
    /// Contracts to perform REST API calls
    /// </summary>
    public interface IRestRepository
    {
        /// <summary>
        /// Performs a REST request with no data and no response
        /// </summary>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Operation status result</returns>
        Task MakeRequestAsync(string request, HttpVerb verb, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data and no response
        /// </summary>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Operation status result</returns>
        Task MakeRequestAsync(string request, HttpVerb verb, object data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="apiVersion">Custom header api version</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, string apiVersion, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, object data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="apiVersion">Custom header api version</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, object data, string apiVersion, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, object data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="verb">HTTP verb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="apiVersion">Custom header api version</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, object data, string apiVersion, CancellationToken cancellationToken = default);
    }
}