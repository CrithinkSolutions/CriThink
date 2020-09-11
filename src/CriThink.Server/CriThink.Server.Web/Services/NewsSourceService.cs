using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Web.Exceptions;
using MediatR;

namespace CriThink.Server.Web.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public NewsSourceService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddSourceAsync(NewsSourceAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = new Uri(request.Uri, UriKind.Absolute);
            var authenticity = _mapper.Map<NewsSourceClassification, NewsSourceAuthenticity>(request.Classification);

            var command = new CreateNewsSourceCommand(uri, authenticity);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveBadSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var command = new RemoveBadNewsSourceCommand(uri);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveGoodNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var command = new RemoveGoodNewsSourceCommand(uri);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var query = new SearchNewsSourceQuery(uri);
            var queryResponse = await _mediator.Send(query).ConfigureAwait(false);

            if (queryResponse is SearchNewsSourceQueryResponse searchResponse)
            {
                var response = _mapper.Map<SearchNewsSourceQueryResponse, NewsSourceSearchResponse>(searchResponse);
                return response;
            }

            throw new ResourceNotFoundException($"The given source {uri} doesn't exist");
        }

        public async Task<IList<NewsSourceGetAllResponse>> GetAllNewsSourcesAsync(NewsSourceGetAllFilterRequest request)
        {
            var sourceFilter = _mapper.Map<NewsSourceGetAllFilterRequest, GetAllNewsSourceFilter>(request);

            var query = new GetAllNewsSourceQuery(sourceFilter);
            var queryResponse = await _mediator.Send(query).ConfigureAwait(false);

            if (queryResponse is IEnumerable<GetAllNewsSourceQueryResponse> allNewsSources)
            {
                var response = _mapper.Map<IEnumerable<GetAllNewsSourceQueryResponse>, IEnumerable<NewsSourceGetAllResponse>>(allNewsSources);
                return response.ToList();
            }

            throw new Exception("An error is occurred");
        }
    }

    public interface INewsSourceService
    {
        /// <summary>
        /// Add the given source to the black or whitelist
        /// </summary>
        /// <param name="request">Source with the classification</param>
        /// <returns>A task</returns>
        Task AddSourceAsync(NewsSourceAddRequest request);

        /// <summary>
        /// Remove a source from the blacklist
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A task</returns>
        Task RemoveBadSourceAsync(Uri uri);

        /// <summary>
        /// Remove a source from the whitelist
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A task</returns>
        Task RemoveGoodNewsSourceAsync(Uri uri);

        /// <summary>
        /// Search the given source in the black and whitelist
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns></returns>
        Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri);

        /// <summary>
        /// Get all the news sources stored. Result can be filtered
        /// </summary>
        /// <param name="request">Optional filter</param>
        /// <returns>All the news sources</returns>
        Task<IList<NewsSourceGetAllResponse>> GetAllNewsSourcesAsync(NewsSourceGetAllFilterRequest request);
    }
}
