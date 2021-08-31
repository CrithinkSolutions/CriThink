using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class DebunkingNewsQueries : IDebunkingNewsQueries
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IDebunkingNewsRepository _debunkingNewsRepository;
        private readonly ILogger<DebunkingNewsQueries> _logger;

        public DebunkingNewsQueries(
            IMapper mapper,
            IDebunkingNewsRepository debunkingNewsRepository,
            IConfiguration configuration,
            ILogger<DebunkingNewsQueries> logger)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _connectionString = configuration?.GetConnectionString("CriThinkDbPgSqlConnection") ??
                throw new ArgumentNullException(nameof(configuration));

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

        public async Task<IList<GetAllDebunkingNewsByKeywordsQueryResult>> GetAllDebunkingNewsByKeywordsAsync(
            IEnumerable<string> keywords)
        {
            _logger?.LogInformation(nameof(GetAllDebunkingNewsByKeywordsAsync));

            try
            {
                if (!keywords.Any())
                    return new List<GetAllDebunkingNewsByKeywordsQueryResult>();

                var result = await _debunkingNewsRepository.GetAllDebunkingNewsByKeywordsAsync(keywords);

                _logger?.LogInformation($"{nameof(GetAllDebunkingNewsByKeywordsAsync)}: done");

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting debunking news by keywords");
                throw;
            }
        }

        public async Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(Guid id)
        {
            _logger?.LogInformation(nameof(GetDebunkingNewsAsync));

            var debunkingNews = await _debunkingNewsRepository.GetDebunkingNewsByIdAsync(id);

            var dto = _mapper.Map<DebunkingNews, DebunkingNewsGetDetailsResponse>(debunkingNews);

            _logger?.LogInformation($"{nameof(GetDebunkingNewsAsync)}: done");

            return dto;
        }
    }
}
