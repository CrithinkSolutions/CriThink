using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.HttpRepository;

namespace CriThink.Client.Core.Services
{
    public class DebunkingNewsService : IDebunkingNewsService
    {
        private readonly IRestRepository _restRepository;

        public DebunkingNewsService(IRestRepository restRepository)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
        }

        public async Task<IList<DebunkingNewsGetAllResponse>> GetRecentDebunkingNewsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var request = new DebunkingNewsGetAllRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var debunkingNewsCollection = await _restRepository.MakeRequestAsync<IList<DebunkingNewsGetAllResponse>>(
                    $"{EndpointConstants.DebunkNewsBase}{EndpointConstants.DebunkingNewsGetAll}?{request.ToQueryString()}",
                    HttpRestVerb.Get,
                    cancellationToken)
                .ConfigureAwait(false);

            return debunkingNewsCollection;
        }

        public async Task<DebunkingNewsGetResponse> GetDebunkingNewsByIdAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DebunkingNewsGetRequest
            {
                Id = Guid.Parse(id)
            };

            var debunkingNewsDetails = await _restRepository.MakeRequestAsync<DebunkingNewsGetResponse>(
                    $"{EndpointConstants.DebunkNewsBase}?{request.ToQueryString()}",
                    HttpRestVerb.Get,
                    cancellationToken)
                .ConfigureAwait(false);

            return debunkingNewsDetails;
        }
    }

    public interface IDebunkingNewsService
    {
        /// <summary>
        /// Retrieve latest debunking news
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Number of debunking news per page</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<IList<DebunkingNewsGetAllResponse>> GetRecentDebunkingNewsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Returns details of the given debunking news id
        /// </summary>
        /// <param name="id">Debunking news id</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<DebunkingNewsGetResponse> GetDebunkingNewsByIdAsync(string id, CancellationToken cancellationToken);
    }
}
