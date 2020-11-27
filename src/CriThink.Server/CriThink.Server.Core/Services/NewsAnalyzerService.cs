using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Facades;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    public class NewsAnalyzerService : INewsAnalyzerService
    {
        private readonly INewsScraperManager _newsScraperManager;
        private readonly INewsAnalyzerFacade _newsAnalyzerFacade;
        private readonly IDomainAnalyzerFacade _domainAnalyzerFacade;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<NewsAnalyzerService> _logger;

        public NewsAnalyzerService(INewsScraperManager newsScraperManager, INewsAnalyzerFacade newsAnalyzerFacade, IDomainAnalyzerFacade domainAnalyzerFacade, IMediator mediator, IMapper mapper, ILogger<NewsAnalyzerService> logger)
        {
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
            _newsAnalyzerFacade = newsAnalyzerFacade ?? throw new ArgumentNullException(nameof(newsAnalyzerFacade));
            _domainAnalyzerFacade = domainAnalyzerFacade ?? throw new ArgumentNullException(nameof(domainAnalyzerFacade));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<NewsAnalyzerResponse> HasUriHttpsSupportAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponse = await _domainAnalyzerFacade.HasHttpsSupportAsync(uri).ConfigureAwait(false);
            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<DomainAnalysisProviderResult, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse> AnalyzeDomainAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponse = await _domainAnalyzerFacade.GetDomainInfoAsync(uri).ConfigureAwait(false);

            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<DomainAnalysisProviderResult, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse[]> GetCompleteAnalysisAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponses = await _domainAnalyzerFacade.GetCompleteAnalysisAsync(uri).ConfigureAwait(false);

            return analysisResponses
                .Select(r => _mapper.Map<DomainAnalysisProviderResult, NewsAnalyzerResponse>(r))
                .ToArray();
        }

        public async Task<ScrapeNewsResponse> ScrapeNewsAsync(Uri uri)
        {
            var scraperResponse = await _newsScraperManager.ScrapeNewsWebPage(uri).ConfigureAwait(false);

            var response = _mapper.Map<NewsScraperProviderResponse, ScrapeNewsResponse>(scraperResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse> AnalyzeNewsSentimentAsync(Uri uri)
        {
            var scraperResponse = await _newsScraperManager.ScrapeNewsWebPage(uri).ConfigureAwait(false);

            var analysisResponse = await _newsAnalyzerFacade.GetNewsSentimentAsync(scraperResponse).ConfigureAwait(false);
            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<NewsAnalysisProviderResult, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<IList<DemoNewsResponse>> GetDemoNewsListAsync()
        {
            var query = new GetAllDemoNewsQuery();
            var response = await _mediator.Send(query).ConfigureAwait(false);

            if (response is IEnumerable<DemoNews> newsList)
            {
                return _mapper.Map<IEnumerable<DemoNews>, IList<DemoNewsResponse>>(newsList);
            }

            throw new InvalidOperationException($"Invalid result from '{nameof(GetAllDemoNewsQuery)}' query");
        }

        public async Task AddDemoNewsAsync(DemoNewsAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var entity = _mapper.Map<DemoNewsAddRequest, DemoNews>(request);

            var _ = await _mediator.Send(entity).ConfigureAwait(false);
        }

        public async Task AddQuestionAsync(QuestionAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var entity = _mapper.Map<QuestionAddRequest, Question>(request);

            var _ = await _mediator.Send(entity).ConfigureAwait(false);
        }

        public async Task<IList<QuestionResponse>> GetQuestionListAsync()
        {
            var query = new GetAllQuestionsQuery();
            var response = await _mediator.Send(query).ConfigureAwait(false);

            if (response is IEnumerable<Question> questions)
            {
                return _mapper.Map<IEnumerable<Question>, IList<QuestionResponse>>(questions)
                    .OrderBy(q => q.Order)
                    .ToList();
            }

            throw new InvalidOperationException($"Invalid result from '{nameof(GetAllQuestionsQuery)}' query");
        }

        public async Task<IList<QuestionAnswerResponse>> CompareAnswersAsync(AnswerQuestionsRequest request)
        {
            if (request?.Answers == null || !request.Answers.Any())
                throw new ArgumentNullException(nameof(request));

            var questionToRetrieve = request.Answers
                .Select(aq => aq.QuestionId);

            var query = new GetAllQuestionAnswerQuery(request.NewsId, questionToRetrieve);
            var response = await _mediator.Send(query).ConfigureAwait(false);

            if (response is IList<QuestionAnswer> anwsers)
            {
                var comparedAnswers = new List<QuestionAnswerResponse>();

                foreach (var givenAnswer in request.Answers)
                {
                    var correctAnswer = anwsers.FirstOrDefault(a => a.Question.Id == givenAnswer.QuestionId);
                    if (correctAnswer == null)
                    {
                        _logger.LogWarning($"Given question not found in DB. Id: '{givenAnswer.QuestionId}'", givenAnswer);
                        continue;
                    }

                    var comparedAnswer = new QuestionAnswerResponse
                    {
                        QuestionId = correctAnswer.Question.Id.ToString(),
                        IsCorrect = correctAnswer.IsPositive == givenAnswer.IsPositive
                    };

                    comparedAnswers.Add(comparedAnswer);
                }

                return comparedAnswers;
            }

            throw new InvalidOperationException($"Invalid result from '{nameof(GetAllQuestionAnswerQuery)}' query");
        }

        public async Task AddAnswerAsync(QuestionAnswerAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var entity = _mapper.Map<QuestionAnswerAddRequest, QuestionAnswer>(request);

            var questionQuery = new GetQuestionQuery(request.QuestionId);
            var newsQuery = new GetDemoNewsQuery(request.NewsId);

            entity.Question = await _mediator.Send(questionQuery).ConfigureAwait(false);
            entity.DemoNews = await _mediator.Send(newsQuery).ConfigureAwait(false);

            var _ = await _mediator.Send(entity).ConfigureAwait(false);
        }
    }
}
