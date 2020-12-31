using System.Threading;
using System.Threading.Tasks;

namespace CriThink.Common.HttpRepository
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
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Operation status result</returns>
        Task MakeRequestAsync(string request, HttpRestVerb restVerb, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data and no response
        /// </summary>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task MakeRequestAsync(string request, HttpRestVerb restVerb, string httpClientName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data and no response
        /// </summary>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="apiVersion">Custom header api version</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task MakeRequestAsync(string request, HttpRestVerb restVerb, string httpClientName, string apiVersion, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data and no response
        /// </summary>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Operation status result</returns>
        Task MakeRequestAsync(string request, HttpRestVerb restVerb, object data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with no data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="apiVersion">Custom header api version</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, string apiVersion, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, object data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="apiVersion">Custom header api version</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, object data, string apiVersion, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, object data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a REST request with data
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="request">Endpoint</param>
        /// <param name="restVerb">HTTP restVerb</param>
        /// <param name="httpClientName">Custom HttpClient name</param>
        /// <param name="data">Data to serialize and send with the request</param>
        /// <param name="apiVersion">Custom header api version</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>REST response body</returns>
        Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, object data, string apiVersion, CancellationToken cancellationToken = default);
    }
}
