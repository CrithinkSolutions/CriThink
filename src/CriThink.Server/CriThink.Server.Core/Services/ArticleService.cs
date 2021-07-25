using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Article;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    internal class ArticleService : IArticleService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly INewsSourceService _newsSourceService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(IHttpContextAccessor httpContext, IMediator mediator, IMapper mapper, INewsSourceService newsSourceService, IUserRepository userRepository, ILogger<ArticleService> logger)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger;
        }

        public async Task<ArticleGetAllQuestionsResponse> GetQuestionsAsync()
        {
            var query = new GetAllQuestionsQuery();
            var questions = await _mediator.Send(query);

            var response = _mapper.Map<GetAllArticleQuestionsResponse, ArticleGetAllQuestionsResponse>(questions);

            return response;
        }

        public async Task<ArticlePostAnswersResponse> PostAnswersToArticleQuestionsAsync(ArticlePostAllAnswersRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = _httpContext.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                throw new InvalidOperationException("User is not logged in");

            var user = await _userRepository.FindUserAsync(userId, false);
            if (user is null)
                throw new InvalidOperationException("User not found");

            var newsLink = request.NewsLink;

            var userAnswerQuery = new GetAllArticleAnswersByUserIdAndNewsLinkQuery(userId, newsLink);
            var userOldAnswers = await _mediator.Send(userAnswerQuery);

            if (userOldAnswers is not null && userOldAnswers.Any())
            {
                _logger?.LogWarning("This user already gave a rate for this news", userId, newsLink);
                throw new InvalidOperationException("This user has already gave a rate for this news");
            }

            var questionsQuery = new GetAllQuestionsQuery();
            var questions = await _mediator.Send(questionsQuery);

            var newsSourceSearch = await _newsSourceService.SearchNewsSourceWithAlertAsync(request.NewsLink);

            var answerByNewsLink = new GetAllArticleAnswersByNewsLinkQuery(request.NewsLink);
            var otherUserAnswers = await _mediator.Send(answerByNewsLink);

            var answer = new ArticleAnswer(request.NewsLink, user);
            answer.CalculateUserRate(newsSourceSearch.Classification, questions.Questions, request.Questions.Select(q => (q.QuestionId, q.Rate)).ToList());

            var communityRate = otherUserAnswers.Any() ?
                otherUserAnswers.Average(oua => oua.Rate) :
                answer.Rate;

            var saveAnswerCommand = new CreateArticleAnswerCommand(answer);
            await _mediator.Send(saveAnswerCommand);

            var response = new ArticlePostAnswersResponse
            {
                NewsSource = newsSourceSearch,
                UserRate = answer.Rate,
                CommunityRate = communityRate,
            };

            return response;
        }
    }
}
