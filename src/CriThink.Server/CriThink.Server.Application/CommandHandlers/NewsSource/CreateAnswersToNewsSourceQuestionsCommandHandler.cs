﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Localize;
using CriThink.Server.Application.Validators;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.QueryResults;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Providers.NewsAnalyzer.Services;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CreateAnswersToNewsSourceQuestionsCommandHandler : IRequestHandler<CreateAnswersToNewsSourceQuestionsCommand, NewsSourcePostAnswersResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly INewsSourceAnswersRepository _newsSourceAnswersRepository;
        private readonly INewsSourceQuestionRepository _newsSourceQuestionRepository;
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly IDebunkingNewsRepository _debunkingNewsRepository;
        private readonly INewsSourceCategoriesRepository _newsSourceCategoriesRepository;
        private readonly IUnknownNewsSourceRepository _unknownNewsSourcesRepository;
        private readonly IMapper _mapper;
        private readonly INewsScraperService _scraperService;
        private readonly ITextAnalyticsService _textAnalyticsService;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly ILogger<CreateAnswersToNewsSourceQuestionsCommandHandler> _logger;

        public CreateAnswersToNewsSourceQuestionsCommandHandler(
            IUserRepository userRepository,
            INewsSourceAnswersRepository newsSourceAnswersRepository,
            INewsSourceQuestionRepository newsSourceQuestionRepository,
            INewsSourceRepository newsSourceRepository,
            IDebunkingNewsRepository debunkingNewsRepository,
            INewsSourceCategoriesRepository newsSourceCategoriesRepository,
            IUnknownNewsSourceRepository unknownNewsSourcesRepository,
            IMapper mapper,
            INewsScraperService scraperService,
            ITextAnalyticsService textAnalyticsService,
            IStringLocalizer<SharedResource> stringLocalizer,
            ILogger<CreateAnswersToNewsSourceQuestionsCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _newsSourceAnswersRepository = newsSourceAnswersRepository ??
                throw new ArgumentNullException(nameof(newsSourceAnswersRepository));

            _newsSourceQuestionRepository = newsSourceQuestionRepository ??
                throw new ArgumentNullException(nameof(newsSourceQuestionRepository));

            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _newsSourceCategoriesRepository = newsSourceCategoriesRepository ??
                throw new ArgumentNullException(nameof(newsSourceCategoriesRepository));

            _unknownNewsSourcesRepository = unknownNewsSourcesRepository ??
                throw new ArgumentNullException(nameof(unknownNewsSourcesRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _scraperService = scraperService ??
                throw new ArgumentNullException(nameof(scraperService));

            _textAnalyticsService = textAnalyticsService ??
                throw new ArgumentNullException(nameof(textAnalyticsService));

            _stringLocalizer = stringLocalizer ??
                throw new ArgumentNullException(nameof(stringLocalizer));

            _logger = logger;
        }

        public async Task<NewsSourcePostAnswersResponse> Handle(
            CreateAnswersToNewsSourceQuestionsCommand request,
            CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(CreateAnswersToNewsSourceQuestionsCommandHandler));

            IReadOnlyCollection<NewsSourceRelatedDebunkingNewsResponse> relatedDebunkingNews = null;

            var userId = request.UserId;
            var newsLink = request.NewsLink;
            var domain = GetDomainFromNewsLink(newsLink);

            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
                throw new CriThinkNotFoundException("User not found", userId);

            await ThrowExceptionIfUserHasAlreadyAnswersAsync(userId, newsLink);

            var searchResponse = await _newsSourceRepository.SearchNewsSourceAsync(domain);
            if (searchResponse is null)
            {
                await HandleUnknownDomainAsync(user, newsLink, cancellationToken);
                return PrepareResponse();
            }

            user.AddSearch(newsLink, searchResponse.Category);

            var (userRate, communityRate) = await CalculateNewsRatesAsync(
                user,
                newsLink,
                searchResponse,
                request.Questions);

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            if (searchResponse.Category == NewsSourceAuthenticity.Conspiracist ||
                searchResponse.Category == NewsSourceAuthenticity.FakeNews ||
                searchResponse.Category == NewsSourceAuthenticity.Suspicious)
            {
                relatedDebunkingNews = await GetRelatedDebunkingNewsCollectionAsync(newsLink);
            }

            var newsSourceCategoryDescription = await _newsSourceCategoriesRepository.GetDescriptionByCategoryNameAsync(searchResponse.Category);

            var response = PrepareResponse(
                relatedDebunkingNews,
                newsSourceCategoryDescription,
                userRate,
                communityRate,
                searchResponse.Category);

            _logger?.LogInformation($"{nameof(CreateAnswersToNewsSourceQuestionsCommandHandler)}: done");

            return response;
        }

        private async Task ThrowExceptionIfUserHasAlreadyAnswersAsync(
            Guid userId, string newsLink)
        {
            var userOldAnswers = await _newsSourceAnswersRepository.GetNewsSourceAnswersByUserId(
                userId,
                newsLink);

            if (userOldAnswers?.Any() == true)
            {
                _logger?.LogWarning("This user already gave a rate for this news", userId, newsLink);
                throw new InvalidOperationException("This user has already gave a rate for this news");
            }
        }

        private async Task HandleUnknownDomainAsync(
            User user,
            string newsLink,
            CancellationToken cancellationToken)
        {
            user.AddSearch(newsLink, NewsSourceAuthenticity.Unknown);
            await _unknownNewsSourcesRepository.AddUnknownNewsSourceAsync(newsLink);

            await _unknownNewsSourcesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }

        private async Task<IReadOnlyCollection<NewsSourceRelatedDebunkingNewsResponse>> GetRelatedDebunkingNewsCollectionAsync(
            string newsLink)
        {
            var newsKeywords = await GetNewsKeywordsAsync(newsLink);
            if (newsKeywords is null)
                return null;

            var dNewsByKeywordsQuery = await _debunkingNewsRepository.GetAllDebunkingNewsByKeywordsAsync(newsKeywords);

            var dto = _mapper.Map<IList<GetAllDebunkingNewsByKeywordsQueryResult>, IReadOnlyCollection<NewsSourceRelatedDebunkingNewsResponse>>(dNewsByKeywordsQuery);
            return dto;
        }

        private async Task<IReadOnlyList<string>> GetNewsKeywordsAsync(string newsLink)
        {
            try
            {
                var uri = new Uri(newsLink, UriKind.Absolute);
                var scrapeAnalysis = await _scraperService.ScrapeNewsWebPage(uri);

                var keywords = await _textAnalyticsService.GetKeywordsFromTextAsync(
                    scrapeAnalysis.NewsBody,
                    scrapeAnalysis.Language);

                return keywords;
            }
            catch (InvalidOperationException ex)
            {
                _logger?.LogWarning(ex, "The given URL is not readable", newsLink);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error getting debunking news by keywords '{newsLink}'");
            }

            return null;
        }

        private async Task<(decimal userRate, decimal communityRate)> CalculateNewsRatesAsync(
            User user,
            string newsLink,
            NewsSource searchResponse,
            IList<NewsSourcePostAnswerRequest> answers)
        {
            var questionList = await _newsSourceQuestionRepository.GetQuestionsByCategoryAsync(QuestionCategory.General);
            var otherUserAnswers = await _newsSourceAnswersRepository.GetNewsSourceAnswersByNewsLinkAsync(newsLink);

            var answer = NewsSourcePostAnswer.Create(newsLink, user);
            answer.CalculateUserRate(searchResponse.Category, questionList, answers.Select(q => (q.QuestionId, q.Rate)).ToList());

            var communityRate = otherUserAnswers.Any() ?
                otherUserAnswers.Average(oua => oua.Rate) :
                answer.Rate;

            await _newsSourceAnswersRepository.AddAsync(answer);

            user.AddAnswer(answer);

            return (answer.Rate, communityRate);
        }

        private NewsSourcePostAnswersResponse PrepareResponse(
            IReadOnlyCollection<NewsSourceRelatedDebunkingNewsResponse> relatedDebunkingNews = null,
            string description = null,
            decimal? userRate = null,
            decimal? communityRate = null,
            NewsSourceAuthenticity authenticity = NewsSourceAuthenticity.Unknown)
        {
            NewsSourcePostAnswersResponse response = new()
            {
                UserRate = userRate,
                CommunityRate = communityRate,
                Classification = _mapper.Map<NewsSourceAuthenticity, NewsSourceAuthenticityDto>(authenticity),
                RelatedDebunkingNews = relatedDebunkingNews,
            };

            if (description is not null)
                response.Description = _stringLocalizer[description];

            return response;
        }

        private static string GetDomainFromNewsLink(string newsLink)
        {
            var resolver = new DomainValidator();
            return resolver.ValidateDomain(newsLink);
        }
    }
}