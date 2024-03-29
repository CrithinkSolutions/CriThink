﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;
using CriThink.Server.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class DebunkingNewsQueries : IDebunkingNewsQueries
    {
        private readonly IMapper _mapper;
        private readonly IDebunkingNewsRepository _debunkingNewsRepository;
        private readonly ILogger<DebunkingNewsQueries> _logger;

        public DebunkingNewsQueries(
            IMapper mapper,
            IDebunkingNewsRepository debunkingNewsRepository,
            ILogger<DebunkingNewsQueries> logger)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _logger = logger;
        }

        public async Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(
            int pageSize,
            int pageIndex,
            string languageFilter = null)
        {
            _logger?.LogInformation(nameof(GetAllDebunkingNewsAsync));

            var debunkingNewsCollection = await _debunkingNewsRepository.GetAllDebunkingNewsAsync(
                pageSize,
                pageIndex,
                languageFilter);

            var dtos = debunkingNewsCollection
                .Take(pageSize)
                .Select(debunkingNews => _mapper.Map<GetAllDebunkingNewsQueryResult, DebunkingNewsGetResponse>(debunkingNews))
                .ToList();

            var response = new DebunkingNewsGetAllResponse(dtos, debunkingNewsCollection.Count > pageSize);

            _logger?.LogInformation($"{nameof(GetAllDebunkingNewsAsync)}: done");

            return response;
        }

        public async Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsByIdAsync(Guid id)
        {
            _logger?.LogInformation(nameof(GetDebunkingNewsByIdAsync));

            var debunkingNews = await _debunkingNewsRepository.GetDebunkingNewsByIdAsync(id);

            var dto = _mapper.Map<DebunkingNews, DebunkingNewsGetDetailsResponse>(debunkingNews);

            _logger?.LogInformation($"{nameof(GetDebunkingNewsByIdAsync)}: done");

            return dto;
        }

        public async Task<IEnumerable<NewsSourceRelatedDebunkingNewsResponse>> SearchByTextAsync(
            string query)
        {
            var keywords = query.Split(null);

            var dNewsByKeywordsQuery = await _debunkingNewsRepository.GetAllDebunkingNewsByKeywordsAsync(keywords);
            var dto = _mapper.Map<IList<GetAllDebunkingNewsByKeywordsQueryResult>, IReadOnlyCollection<NewsSourceRelatedDebunkingNewsResponse>>(dNewsByKeywordsQuery);

            return dto;
        }
    }
}
