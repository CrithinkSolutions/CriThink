﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Services
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
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var command = new RemoveNewsSourceCommand(uri);
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var query = new SearchNewsSourceQuery(uri);
            var queryResponse = await _mediator.Send(query).ConfigureAwait(false);

            if (queryResponse != null)
            {
                var response = _mapper.Map<SearchNewsSourceQueryResponse, NewsSourceSearchResponse>(queryResponse);
                return response;
            }

            throw new ResourceNotFoundException($"The given source {uri} doesn't exist");
        }

        public async Task<NewsSourceGetAllResponse> GetAllNewsSourcesAsync(NewsSourceGetAllRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var sourceFilter = _mapper.Map<NewsSourceGetAllFilterRequest, GetAllNewsSourceFilter>(request.Filter);

            var query = new GetAllNewsSourceQuery(request.PageSize, request.PageIndex, sourceFilter);
            var queryResponse = await _mediator.Send(query).ConfigureAwait(false);

            var dtos = new List<NewsSourceGetResponse>();
            foreach (var newsSource in queryResponse.Take(request.PageSize))
            {
                var dto = _mapper.Map<GetAllNewsSourceQueryResponse, NewsSourceGetResponse>(newsSource);
                dtos.Add(dto);
            }

            var response = new NewsSourceGetAllResponse(dtos, queryResponse.Count > request.PageSize);
            return response;
        }
    }
}
