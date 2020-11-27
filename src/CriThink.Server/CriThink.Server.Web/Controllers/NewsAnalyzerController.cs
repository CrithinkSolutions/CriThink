using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Common;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to offer services to analyze news and relative URLs
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiValidationFilter]
    [ApiController]
    [Route(EndpointConstants.ApiBase + EndpointConstants.NewsAnalyzerBase)] //api/news-analyzer
    public class NewsAnalyzerController : Controller
    {
        private readonly INewsAnalyzerService _newsAnalyzerService;

        public NewsAnalyzerController(INewsAnalyzerService newsAnalyzerService)
        {
            _newsAnalyzerService = newsAnalyzerService ?? throw new ArgumentNullException(nameof(newsAnalyzerService));
        }

        /// <summary>
        /// Perform all the available news analysis
        /// </summary>
        /// <param name="request">The URI to analyze</param>
        /// <returns>A response with the result of all the performed analysis</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.NewsAnalyzerPerformCompleteAnalysis)] // api/news-analyzer/perform-complete-anlysis
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> CompleteAnalysisAsync([FromBody] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            var responses = await _newsAnalyzerService.GetCompleteAnalysisAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(responses));
        }

        /// <summary>
        /// Analyze the given <see cref="Uri"/> and indicate if it has the HTTPS support
        /// </summary>
        /// <param name="request">The URI to analyze</param>
        /// <returns>Provide the analysis result</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.NewsAnalyzerHttpsSupport)] // api/news-analyzer/https-support
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> HasHttpsSupportAsync([FromBody] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            var hasHttpsSupport = await _newsAnalyzerService.HasUriHttpsSupportAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(hasHttpsSupport));
        }

        /// <summary>
        /// Analyze the domain of the given <see cref="Uri"/>
        /// </summary>
        /// <param name="request">The URI to analyze</param>
        /// <returns>Provide the analysis result</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.NewsAnalyzerDomainLookup)] // api/news-analyzer/domain-lookup
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> DomainLookupAsync([FromBody] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            var domainInfoResponse = await _newsAnalyzerService.AnalyzeDomainAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(domainInfoResponse));
        }

        /// <summary>
        /// Parse the news of the given URL and returns news info
        /// </summary>
        /// <param name="request">The news URL</param>
        /// <returns>News information such as author and body</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.NewsAnalyzerScrapeNews)] // api/news-analyzer/scrape-news
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ScrapeNewsAsync([FromBody] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            var response = await _newsAnalyzerService.ScrapeNewsAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Analyze the given news content and gives scores back
        /// </summary>
        /// <param name="request">News uri</param>
        /// <returns>News sentiment scores</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.NewsAnalyzerTextSentimentAnalysis)] // api/news-analyzer/sentiment-analysis
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AnalyzeNewsSentimentAsync([FromBody] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            var response = await _newsAnalyzerService.AnalyzeNewsSentimentAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Add the given question
        /// </summary>
        /// <param name="request">Question to add</param>
        /// <returns></returns>
        [Authorize]
        [Route(EndpointConstants.NewsAnalyzerQuestionAdd)] // api/news-analyzer/question
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AddQuestionAsync([FromBody] QuestionAddRequest request)
        {
            await _newsAnalyzerService.AddQuestionAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Returns a predefined list of questions for the user
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 30)]
        [Route(EndpointConstants.NewsAnalyzerQuestionGetAll)] // api/news-analyzer/question
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetQuestionListAsync()
        {
            var newsList = await _newsAnalyzerService.GetQuestionListAsync().ConfigureAwait(false);
            return Ok(new ApiOkResponse(newsList));
        }

        /// <summary>
        /// Compare the given answers to the correct ones
        /// </summary>
        /// <param name="request">Answer to compare</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.NewsAnalyzerQuestionAnswer)] // api/news-analyzer/answer-question
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AnwserQuestionAsync([FromBody] AnswerQuestionsRequest request)
        {
            var results = await _newsAnalyzerService.CompareAnswersAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(results));
        }

        /// <summary>
        /// Add an answer question for a specific news
        /// </summary>
        /// <param name="request">Anwser to add</param>
        /// <returns></returns>
        [Authorize]
        [Route(EndpointConstants.NewsAnalyzerQuestionAnswerAdd)] // api/news-analyzer/answer-question/create
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AddQuestionAnswerAsync([FromBody] QuestionAnswerAddRequest request)
        {
            await _newsAnalyzerService.AddAnswerAsync(request).ConfigureAwait(false);
            return NoContent();
        }
    }
}
