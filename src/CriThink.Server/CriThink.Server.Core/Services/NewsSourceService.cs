using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;

namespace CriThink.Server.Core.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;

        public NewsSourceService(IMediator mediator, IMapper mapper, IEmailSenderService emailSenderService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
        }

        public async Task AddSourceAsync(NewsSourceAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = new Uri(request.Uri, UriKind.Absolute);
            var authenticity = _mapper.Map<NewsSourceClassification, NewsSourceAuthenticity>(request.Classification);

            var command = new CreateNewsSourceCommand(uri, authenticity);
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveBadSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var command = new RemoveBadNewsSourceCommand(uri);
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveGoodNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var command = new RemoveGoodNewsSourceCommand(uri);
            _ = await _mediator.Send(command).ConfigureAwait(false);
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
            else if (queryResponse is null)
            {
                return null;
            }

            throw new ResourceNotFoundException($"The given source {uri} doesn't exist");
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceWithAlertAsync(Uri uri)
        {
            var searchResponse = await SearchNewsSourceAsync(uri).ConfigureAwait(false);

            if (searchResponse is null)
            {
                await _emailSenderService.SendUnknownDomainAlertEmailAsync(uri.ToString()).ConfigureAwait(false);
                var command = new CreateUnknownNewsSourceCommand(uri);
                await _mediator.Send(command).ConfigureAwait(false);
            }

            return searchResponse;
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
}
