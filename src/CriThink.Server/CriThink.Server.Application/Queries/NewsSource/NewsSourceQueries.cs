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
using Microsoft.Extensions.Logging;

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

        public NewsSourceQueries(
            IMapper mapper,
            INewsSourceQuestionRepository newsSourceQuestionRepository,
            IUnknownNewsSourceRepository unknownNewsSourcesRepository,
            INewsSourceRepository newsSourceRepository,
            INewsSourceCategoriesRepository newsSourceCategoriesRepository,
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
                Classification = _mapper.Map<NewsSourceAuthenticity, NewsSourceClassification>(newsSource.Category),
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
