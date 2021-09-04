using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Providers.EmailSender.Services;
using CriThink.Server.Providers.NewsAnalyzer.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;
        private readonly INewsScraperService _scraperService;
        private readonly ITextAnalyticsService _textAnalyticsService;
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateAnswersToNewsSourceQuestionsCommandHandler> _logger;

        public CreateAnswersToNewsSourceQuestionsCommandHandler(
            IUserRepository userRepository,
            INewsSourceAnswersRepository newsSourceAnswersRepository,
            INewsSourceQuestionRepository newsSourceQuestionRepository,
            INewsSourceRepository newsSourceRepository,
            IDebunkingNewsRepository debunkingNewsRepository,
            INewsSourceCategoriesRepository newsSourceCategoriesRepository,
            IMapper mapper,
            IEmailSenderService emailSenderService,
            INewsScraperService scraperService,
            ITextAnalyticsService textAnalyticsService,
            CriThinkDbContext dbContext,
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

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _emailSenderService = emailSenderService ??
                throw new ArgumentNullException(nameof(emailSenderService));

            _scraperService = scraperService ??
                throw new ArgumentNullException(nameof(scraperService));

            _textAnalyticsService = textAnalyticsService ??
                throw new ArgumentNullException(nameof(textAnalyticsService));

            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));

            _logger = logger;
        }

        public async Task<NewsSourcePostAnswersResponse> Handle(
            CreateAnswersToNewsSourceQuestionsCommand request,
            CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(CreateAnswersToNewsSourceQuestionsCommandHandler));

            var userId = request.UserId;
            var newsLink = request.NewsLink;

            var user = await _userRepository.FindUserAsync(userId.ToString(), cancellationToken);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId.ToString());

            await ThrowExceptionIfUserHasAlreadyAnswersAsync(userId, newsLink);

            var questionList = await _newsSourceQuestionRepository.GetQuestionsByCategoryAsync(QuestionCategory.General);

            var newsSourceSearch = await SearchNewsSourceWithAlertAsync(newsLink);

            var authenticity = newsSourceSearch is null ?
                NewsSourceAuthenticity.Unknown :
                _mapper.Map<NewsSourceClassification, NewsSourceAuthenticity>(newsSourceSearch.Classification);

            user.AddSearch(newsLink, authenticity);

            if (newsSourceSearch?.Classification == NewsSourceClassification.Unknown)
            {
                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                await _emailSenderService.SendUnknownDomainAlertEmailAsync(newsLink, request.Email);
                throw new ResourceNotFoundException("The given news link does not exist");
            }

            var otherUserAnswers = await _dbContext
                    .ArticleAnswers
                    .Where(aa => aa.NewsLink == request.NewsLink)
                    .ToListAsync(cancellationToken);

            var answer = ArticleAnswer.Create(request.NewsLink, user);
            answer.CalculateUserRate(newsSourceSearch.Classification, questionList, request.Questions.Select(q => (q.QuestionId, q.Rate)).ToList());

            var communityRate = otherUserAnswers.Any() ?
                otherUserAnswers.Average(oua => oua.Rate) :
                answer.Rate;

            await _dbContext.ArticleAnswers.AddAsync(answer, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger?.LogInformation("CreateAnswersToArticleQuestions: done");

            var response = new NewsSourcePostAnswersResponse
            {
                NewsSource = newsSourceSearch,
                UserRate = answer.Rate,
                CommunityRate = communityRate,
            };

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

        private async Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceWithAlertAsync(string newsLink)
        {
            var validatedNewsLink = ValidateNewsLink(newsLink);

            var relatedDebunkingNewsResponse = new List<NewsSourceRelatedDebunkingNewsResponse>();

            var searchResponse = await _newsSourceRepository.SearchNewsSourceAsync(validatedNewsLink);
            var newsSourceCategoryDescription = await _newsSourceCategoriesRepository.GetDescriptionByCategoryNameAsync(searchResponse.Category);

            var authenticity = searchResponse.Category;

            if (authenticity == NewsSourceAuthenticity.Conspiracist ||
                authenticity == NewsSourceAuthenticity.FakeNews ||
                authenticity == NewsSourceAuthenticity.Suspicious)
            {
                try
                {
                    var uri = new Uri(newsLink, UriKind.Absolute);
                    var relatedDebunkingNewsCollection = await GetRelatedDebunkingNewsAsync(uri);

                    foreach (var relatedDNews in relatedDebunkingNewsCollection)
                    {
                        var response = _mapper
                            .Map<GetAllDebunkingNewsByKeywordsQueryResult, NewsSourceRelatedDebunkingNewsResponse>(relatedDNews);

                        relatedDebunkingNewsResponse.Add(response);
                    }
                }
                catch (InvalidOperationException) { }
                catch (UriFormatException ex)
                {
                    _logger?.LogError(ex, $"The given url has the wrong format: '{newsLink}'");
                }
            }

            return new NewsSourceSearchWithDebunkingNewsResponse
            {
                Classification = _mapper.Map<NewsSourceAuthenticity, NewsSourceClassification>(searchResponse.Category),
                Description = newsSourceCategoryDescription,
                RelatedDebunkingNews = relatedDebunkingNewsResponse,
            };
        }

        private async Task<IList<GetAllDebunkingNewsByKeywordsQueryResult>> GetRelatedDebunkingNewsAsync(Uri uri)
        {
            try
            {
                var scrapeAnalysis = await _scraperService.ScrapeNewsWebPage(uri);

                var keywords = await _textAnalyticsService.GetKeywordsFromTextAsync(
                    scrapeAnalysis.NewsBody,
                    scrapeAnalysis.Language);

                var dNewsByKeywordsQuery = await _debunkingNewsRepository.GetAllDebunkingNewsByKeywordsAsync(
                    keywords);

                return dNewsByKeywordsQuery;
            }
            catch (InvalidOperationException ex)
            {
                _logger?.LogWarning(ex, "The given URL is not readable", uri.ToString());
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error getting debunking news by keywords '{uri}'");
                return new List<GetAllDebunkingNewsByKeywordsQueryResult>();
            }
        }

        private static string ValidateNewsLink(string newsLink)
        {
            var resolver = new DomainValidator();
            return resolver.ValidateDomain(newsLink);
        }
    }
}
