using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Application.Validators;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace CriThink.Server.Application.Queries
{
    internal class NewsSourceQueries : INewsSourceQueries
    {
        private readonly IMapper _mapper;
        private readonly INewsSourceQuestionRepository _newsSourceQuestionRepository;
        private readonly IUnknownNewsSourceRepository _unknownNewsSourcesRepository;
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly ILogger<NewsSourceQueries> _logger;
        private readonly INewsSourceCategoriesRepository _newsSourceCategoriesRepository;
        private readonly IUserRepository _userRepository;
        private readonly CriThinkDbContext _dbContext;

        public NewsSourceQueries(
            IMapper mapper,
            INewsSourceQuestionRepository newsSourceQuestionRepository,
            IUnknownNewsSourceRepository unknownNewsSourcesRepository,
            INewsSourceRepository newsSourceRepository,
            INewsSourceCategoriesRepository newsSourceCategoriesRepository,
            IUserRepository userRepository,
            CriThinkDbContext dbContext,
            ILogger<NewsSourceQueries> logger)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _newsSourceQuestionRepository = newsSourceQuestionRepository ??
                throw new ArgumentNullException(nameof(newsSourceQuestionRepository));

            _unknownNewsSourcesRepository = unknownNewsSourcesRepository ??
                throw new ArgumentNullException(nameof(unknownNewsSourcesRepository));

            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _newsSourceCategoriesRepository = newsSourceCategoriesRepository ??
                throw new ArgumentNullException(nameof(newsSourceCategoriesRepository));

            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));

            _logger = logger;
        }

        public IList<GetAllNewsSourceQueryResult> GetAllNewsSources(
            int pageSize,
            int pageIndex,
            NewsSourceAuthenticityFilter filter)
        {
            _logger?.LogInformation(nameof(GetAllNewsSources));

            var newsSourceCollection = _newsSourceRepository.GetAllSearchNewsSources();

            IList<GetAllNewsSourceQueryResult> responses = new List<GetAllNewsSourceQueryResult>();

            foreach (var newsSource in ApplyQueryFilter(newsSourceCollection, pageSize, pageIndex, filter))
            {
                var response = new GetAllNewsSourceQueryResult(newsSource.Name, newsSource.Category);
                responses.Add(response);
            }

            _logger?.LogInformation($"{nameof(GetAllNewsSources)}: done");

            return responses;
        }

        public async Task<NewsSourceSearchResponse> GetNewsSourceByNameAsync(string name)
        {
            _logger?.LogInformation(nameof(GetNewsSourceByNameAsync));

            var validatedNewsLink = ValidateNewsLink(name);

            var newsSource = await _newsSourceRepository.SearchNewsSourceAsync(validatedNewsLink);
            if (newsSource is null)
                return null;

            var response = new NewsSourceSearchResponse()
            {
                Classification = _mapper.Map<NewsSourceAuthenticity, NewsSourceAuthenticityDto>(newsSource.Category),
            };

            _logger?.LogInformation($"{nameof(GetNewsSourceByNameAsync)}: done");

            return response;
        }

        public async Task<UnknownNewsSourceResponse> GetUnknownNewsSourceByIdAsync(Guid id)
        {
            _logger?.LogInformation(nameof(GetUnknownNewsSourceByIdAsync));

            var unknownNewsSource = await _unknownNewsSourcesRepository.GetUnknownNewsSourceByIdAsync(id);

            var response = _mapper.Map<UnknownNewsSource, UnknownNewsSourceResponse>(unknownNewsSource);

            _logger?.LogInformation($"{nameof(GetUnknownNewsSourceByIdAsync)}: done");

            return response;
        }

        public async Task<NewsSourceGetAllQuestionsResponse> GetGeneralQuestionsAsync()
        {
            _logger?.LogInformation(nameof(GetGeneralQuestionsAsync));

            var questions = await _newsSourceQuestionRepository.GetQuestionsByCategoryAsync(QuestionCategory.General);

            var model = new GetAllNewsSourceQuestionsQueryResult(questions);

            var response = _mapper.Map<GetAllNewsSourceQuestionsQueryResult, NewsSourceGetAllQuestionsResponse>(model);

            _logger?.LogInformation($"{nameof(GetGeneralQuestionsAsync)}: done");

            return response;
        }

        public async Task<UnknownNewsSourceGetAllViewModel> GetAllUnknownNewsSourcesAsync(int pageSize, int pageIndex)
        {
            _logger?.LogInformation(nameof(GetAllUnknownNewsSourcesAsync));

            var unknownNewsSourceCollection = await _unknownNewsSourcesRepository.GetAllUnknownSourcesAsync(
                pageSize, pageIndex);

            var dtos = unknownNewsSourceCollection
                .Take(pageSize)
                .Select(_mapper.Map<GetAllUnknownSourcesQueryResult, UnknownNewsSourceGetViewModel>)
                .ToList();

            var response = new UnknownNewsSourceGetAllViewModel(dtos, unknownNewsSourceCollection.Count > pageSize);

            _logger?.LogInformation($"{nameof(GetAllUnknownNewsSourcesAsync)}: done");

            return response;
        }

        public async Task<IEnumerable<NewsSourceSearchByTextResponse>> SearchInUserSearchesByTextAsync(
            Guid userId,
            string text)
        {
            var keywords = text.Split(null);

            var query = "SELECT DISTINCT ON (sn.link) sn.link, sn.id, sn.title, sn.fav_icon, us.timestamp\n" +
                        "FROM (\n" +
                        "    SELECT *\n" +
                        "    FROM searched_news sn\n" +
                        "    ORDER BY dnews_get_related_news(sn.keywords, :keywords) DESC\n" +
                        "    ) as sn\n" +
                        "JOIN user_searches us ON sn.id = us.searched_news_id\n" +
                        $"WHERE us.user_id <> '{userId}'\n" +
                        "ORDER BY sn.link, us.timestamp DESC\n" +
                        "LIMIT 5;";

            await using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(query, connection);

            var keywordsPar = new NpgsqlParameter<string[]>("keywords", NpgsqlDbType.Array | NpgsqlDbType.Char)
            {
                Value = keywords.ToArray()
            };

            command.Parameters.Add(keywordsPar);

            var reader = await command.ExecuteReaderAsync();

            var result = new List<NewsSourceSearchByTextResponse>();

            while (await reader.ReadAsync())
            {
                var response = new NewsSourceSearchByTextResponse
                {
                    Id = reader["id"].ToString(),
                    Title = reader["title"].ToString(),
                    NewsLink = reader["link"].ToString(),
                    FavIcon = reader["fav_icon"].ToString(),
                    PublishingDate = reader["timestamp"].ToString(),
                };

                result.Add(response);
            }

            foreach (var dto in result)
            {
                var newsLinkRates = await _userRepository.GetSearchesRateByNewsLinkAsync(userId, dto.NewsLink);
                dto.Rate = newsLinkRates.Average();
            }

            return result;
        }

        private static string ValidateNewsLink(string newsLink)
        {
            var resolver = new DomainValidator();
            return resolver.ValidateDomain(newsLink);
        }

        private static IEnumerable<NewsSource> ApplyQueryFilter(
            IEnumerable<NewsSource> query,
            int size, int index,
            NewsSourceAuthenticityFilter filter)
        {
            return query
                .Skip(size * index)
                .Take(size + 1)
                .Where(ns => IsSameCategory(filter, ns.Category));
        }

        private static bool IsSameCategory(NewsSourceAuthenticityFilter filter, NewsSourceAuthenticity authenticity)
        {
            return filter switch
            {
                NewsSourceAuthenticityFilter.Blacklist => authenticity == NewsSourceAuthenticity.Conspiracist ||
                                                          authenticity == NewsSourceAuthenticity.FakeNews ||
                                                          authenticity == NewsSourceAuthenticity.Suspicious,
                NewsSourceAuthenticityFilter.Whitelist => authenticity == NewsSourceAuthenticity.Reliable ||
                                                          authenticity == NewsSourceAuthenticity.Satirical ||
                                                          authenticity == NewsSourceAuthenticity.SocialMedia,
                _ => true
            };
        }
    }
}
