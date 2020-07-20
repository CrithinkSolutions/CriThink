using System;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
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
            var authenticity = _mapper.Map<NewsSourceClassification, NewsSourceAuthencity>(request.Classification);

            var command = new CreateNewsSourceCommand(uri, authenticity);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveBadSourceAsync(NewsSourceRemoveRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = new Uri(request.Uri);

            var command = new RemoveBadNewsSourceCommand(uri);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveGoodNewsSourceAsync(NewsSourceRemoveRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = new Uri(request.Uri);

            var command = new RemoveGoodNewsSourceCommand(uri);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(NewsSourceSearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = new Uri(request.Uri);

            var command = new SearchNewsSourceQuery(uri);
            var queryResponse = await _mediator.Send(command).ConfigureAwait(false);

            if (queryResponse is SearchNewsSourceResponse searchResponse)
            {
                var classification = _mapper.Map<NewsSourceAuthencity, NewsSourceClassification>(searchResponse.SourceAuthencity);

                return new NewsSourceSearchResponse
                {
                    Classification = classification
                };
            }

            throw new ResourceNotFoundException($"The given source {uri} doesn't exist");
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
        /// <param name="request">Source to remove</param>
        /// <returns>A task</returns>
        Task RemoveBadSourceAsync(NewsSourceRemoveRequest request);

        /// <summary>
        /// Remove a source from the whitelist
        /// </summary>
        /// <param name="request">Source to remove</param>
        /// <returns>A task</returns>
        Task RemoveGoodNewsSourceAsync(NewsSourceRemoveRequest request);

        /// <summary>
        /// Search the given source in the black and whitelist
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<NewsSourceSearchResponse> SearchNewsSourceAsync(NewsSourceSearchRequest request);
    }
}
